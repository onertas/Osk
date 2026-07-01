using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.Beds;
using OskApi.Services.Abstract;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Staff;

namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IHealthFacilityService _hfService;
    private readonly IHealthFacilityTypeService _hfTypeService;
    private readonly IPmService _pmService;
    private readonly IIcBedService _icBedService;
    private readonly IIcBedNameService _icBedNameService;
    private readonly ITemporarayStaffService _tempStaffService;

    public DashboardController(
        IHealthFacilityService hfService,
        IHealthFacilityTypeService hfTypeService,
        IPmService pmService,
        IIcBedService icBedService,
        IIcBedNameService icBedNameService,
        ITemporarayStaffService tempStaffService)
    {
        _hfService = hfService;
        _hfTypeService = hfTypeService;
        _pmService = pmService;
        _icBedService = icBedService;
        _icBedNameService = icBedNameService;
        _tempStaffService = tempStaffService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSummary()
    {
        // 1) Kuruluş sayıları (tipe göre grupla)
        var facilityCounts = await _hfService.GetAll()
            .Include(h => h.HealthFacilityType)
            .GroupBy(h => h.HealthFacilityType!.Name)
            .Select(g => new { typeName = g.Key, count = g.Count() })
            .ToListAsync();

        // 2) Personel sayıları (ünvana/title göre grupla - aynı kişi 1 kere sayılır)
        var personnelCounts = await _pmService.GetAll()
            .Where(pm => pm.Finish == null) // Aktif personeller
            .Include(pm => pm.Branch)
                .ThenInclude(b => b!.Title)
            .GroupBy(pm => pm.Branch!.Title!.Name)
            .Select(g => new { titleName = g.Key, count = g.Select(pm => pm.PersonnelId).Distinct().Count() })
            .ToListAsync();

        // 3) Yoğun bakım yatak sayıları (tipe göre grupla - IcBedName'deki IcBedType)
        var icBedCounts = await _icBedService.GetAll()
            .Where(b => b.IsActive)
            .Include(b => b.IcBedName)
            .GroupBy(b => b.IcBedName!.IcBedType)
            .Select(g => new { bedTypeValue = g.Key.Value, bedTypeName = g.Key.Name, count = g.Sum(x => x.Quantity) })
            .ToListAsync();

        // IcBedType description'ları için mapping
        var icBedResult = icBedCounts.Select(x => new
        {
            bedTypeName = IcBedType.FromValue(x.bedTypeValue).Description,
            x.count
        }).ToList();

        // Toplam sayılar
        var totalFacilities = await _hfService.GetAll().CountAsync();
        var totalPersonnel = await _pmService.GetAll().Where(pm => pm.Finish == null).Select(pm => pm.PersonnelId).Distinct().CountAsync();
        var totalIcBeds = await _icBedService.GetAll().Where(b => b.IsActive).SumAsync(b => b.Quantity);

        var result = new
        {
            facilityCounts,
            personnelCounts,
            icBedCounts = icBedResult,
            totalFacilities,
            totalPersonnel,
            totalIcBeds
        };

        return Ok(Shared.Result.Result<object>.Ok(result, "Dashboard verileri getirildi"));
    }

    /// <summary>
    /// Geçici kadro tablosunda tanımlı ama aktif personeli bulunmayan
    /// ve son ayrılış tarihinden 3 ay geçmiş kadroları döndürür.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTempStaffAlerts()
    {
        var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);

        // Tüm geçici kadroları çek
        var tempStaffList = await _tempStaffService.GetAll()
            .Where(t => t.IsDeleted == false)
            .Include(t => t.HealthFacility)
            .Include(t => t.Branch)
            .Include(t => t.PmType)
            .ToListAsync();

        // Tüm personel hareketlerini çek (aktif + ayrılmış)
        var allMovements = await _pmService.GetAll()
            .Select(pm => new
            {
                pm.HealthFacilityId,
                pm.BranchId,
                pm.PmTypeId,
                pm.Finish
            })
            .ToListAsync();

        var alerts = new List<object>();

        foreach (var ts in tempStaffList)
        {
            // Bu kadro için aktif (Finish == null) personel var mı?
            var hasActive = allMovements.Any(pm =>
                pm.HealthFacilityId == ts.HealthFacilityId &&
                pm.BranchId == ts.BranchId &&
                pm.PmTypeId == ts.PmTypeId &&
                pm.Finish == null);

            if (hasActive) continue; // Aktif personel varsa uyarı yok

            // Bu kadroya ait en son ayrılış tarihini bul
            var lastFinish = allMovements
                .Where(pm =>
                    pm.HealthFacilityId == ts.HealthFacilityId &&
                    pm.BranchId == ts.BranchId &&
                    pm.PmTypeId == ts.PmTypeId &&
                    pm.Finish != null)
                .Max(pm => (DateTime?)pm.Finish);

            // Hiç hareket yoksa ya da son ayrılış 3 aydan uzun süre önceyse uyarı ver
            if (lastFinish == null || lastFinish <= threeMonthsAgo)
            {
                alerts.Add(new
                {
                    tempStaffId = ts.Id,
                    healthFacilityName = ts.HealthFacility!.Name,
                    branchName = ts.Branch!.Name,
                    pmTypeName = ts.PmType!.Name,
                    lastFinishDate = lastFinish,
                    daysSinceFinish = lastFinish.HasValue
                        ? (int)(DateTime.UtcNow - lastFinish.Value).TotalDays
                        : (int?)null
                });
            }
        }

        return Ok(Shared.Result.Result<object>.Ok(alerts, "Geçici kadro uyarıları getirildi"));
    }

    /// <summary>
    /// Sözleşme bitiş tarihi girilmiş ve tarihi geçmiş,
    /// ancak henüz ayrılış kaydı (Finish) işlenmemiş personel hareketlerini döndürür.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetExpiredContractAlerts()
    {
        var today = DateTime.UtcNow;

        // daysOverdue (DateTime farkı) EF Core tarafından SQL'e çevrilemiyor.
        // Bu yüzden önce ham verileri çekiyoruz, sonra in-memory hesaplıyoruz.
        var rawList = await _pmService.GetAll()
            .Where(pm =>
                pm.ContractFinish != null &&       // Sözleşme bitiş tarihi girilmiş
                pm.ContractFinish < today  &&      // Tarihi geçmiş
                pm.Finish == null)                 // Hâlâ aktif (ayrılış yapılmamış)
            .Include(pm => pm.Personnel)
            .Include(pm => pm.HealthFacility)
            .Include(pm => pm.Branch)
            .Include(pm => pm.PmType)
            .OrderBy(pm => pm.ContractFinish)
            .Select(pm => new
            {
                pmId               = pm.Id,
                personnelName      = pm.Personnel!.FirstName + " " + pm.Personnel.LastName,
                healthFacilityName = pm.HealthFacility!.Name,
                branchName         = pm.Branch!.Name,
                pmTypeName         = pm.PmType!.Name,
                contractStart      = pm.ContractStart,
                contractFinish     = pm.ContractFinish
            })
            .ToListAsync();

        // daysOverdue'yi in-memory olarak hesapla
        var expiredList = rawList.Select(pm => new
        {
            pm.pmId,
            pm.personnelName,
            pm.healthFacilityName,
            pm.branchName,
            pm.pmTypeName,
            pm.contractStart,
            pm.contractFinish,
            daysOverdue = (int)(today - pm.contractFinish!.Value).TotalDays
        }).ToList();

        return Ok(Shared.Result.Result<object>.Ok(expiredList, "Sözleşme süresi dolmuş personeller getirildi"));
    }

    /// <summary>
    /// Askı süresi dolmuş (Ruhsat Askı veya Faaliyet Durdurma durumunda olan ve bitiş tarihi geçmiş)
    /// sağlık tesislerini döndürür.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSuspendedFacilityAlerts()
    {
        var today = DateTime.Today;

        var rawList = await _hfService.GetAll()
            .Where(x => (x.HfStatus == HfStatus.RuhsatAski || x.HfStatus == HfStatus.FaaliyetDurdurma)
                        && x.SuspensionEndDate != null
                        && x.SuspensionEndDate < today)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                statusValue = x.HfStatus.Value,
                statusName = x.HfStatus.Name,
                statusDate = x.StatusDate,
                suspensionEndDate = x.SuspensionEndDate
            })
            .ToListAsync();

        var alerts = rawList.Select(x => new
        {
            x.id,
            x.name,
            x.statusValue,
            x.statusName,
            x.statusDate,
            x.suspensionEndDate,
            daysOverdue = (int)(today - x.suspensionEndDate!.Value).TotalDays
        }).ToList();

        return Ok(Shared.Result.Result<object>.Ok(alerts, "Askı süresi dolmuş sağlık tesisleri getirildi"));
    }
}
