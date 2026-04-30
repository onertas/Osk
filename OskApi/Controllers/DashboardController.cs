using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.Beds;
using OskApi.Services.Abstract;

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

    public DashboardController(
        IHealthFacilityService hfService,
        IHealthFacilityTypeService hfTypeService,
        IPmService pmService,
        IIcBedService icBedService,
        IIcBedNameService icBedNameService)
    {
        _hfService = hfService;
        _hfTypeService = hfTypeService;
        _pmService = pmService;
        _icBedService = icBedService;
        _icBedNameService = icBedNameService;
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
}
