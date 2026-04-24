using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.PersonnelMovement;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Rules;

public class PmBusinessRules
{
    private readonly IPmTypeService _pmTypeService;
    private readonly IStaffService _staffService;
    private readonly IPmService _pmService;

    public PmBusinessRules(IPmTypeService pmTypeService, IStaffService staffService, IPmService pmService)
    {
        _pmTypeService = pmTypeService;
        _staffService = staffService;
        _pmService = pmService;
    }

    public async Task<Result<string>> CheckRulesForCreateAsync(CreatePersonelMovementDto model)
    {
        var pmType = await _pmTypeService.GetAll().FirstOrDefaultAsync(t => t.Id == model.PmTypeId);
        
        // Sadece "KAD" (Kadrolu) kodlu tipler için kontrol yapılıyor (İstenirse IsUsingStaff de eklenebilir)
        if (pmType != null && pmType.IsUsingStaff)
        {
            // 1. İlgili kurum ve branş (ünvan) için tanımlı kadroyu getir
            var staff = await _staffService.GetAll()
                .FirstOrDefaultAsync(s => s.HealthFacilityId == model.HealthFacilityId && s.BranchId == model.BranchId);

            if (staff == null)
                return Result<string>.Fail("İlgili tesis ve branş için kadro tanımlı değil. Lütfen önce kadro ekleyiniz.");

            // 2. Aynı kurum ve branşta, "KAD" koduyla halihazırda başlamış personellerin sayısını al
            var activeCount = await _pmService.GetAll()
                .Include(pm => pm.PmType)
                .CountAsync(pm => pm.HealthFacilityId == model.HealthFacilityId 
                               && pm.BranchId == model.BranchId 
                               && pm.PmType != null 
                               && pm.PmType.Code == "KAD"
                               && pm.Start <= DateTime.Now // Başlamış olanlar
                               && pm.Finish == null); // Ve henüz bitiş tarihi gelmemiş / bitmemiş olanlar

            // 3. Kadro sayısı ile mevcut aktif çalışan sayısını karşılaştır
            if (activeCount >= staff.Count)
            {
                return Result<string>.Fail($"Kadro yoktur. İlgili branşta boş kadrolu (KAD) pozisyon bulunmamaktadır. (Kapasite: {staff.Count}, Mevcut Başlamış: {activeCount})");
            }
        }

        // Diğer kurallar buraya eklenebilir...
        // Örneğin: IsUsingStaff = true olan diğer durumlar vs.

        return Result<string>.Ok("Kurallar geçerli");
    }
}
