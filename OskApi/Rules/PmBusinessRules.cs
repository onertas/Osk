using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.PersonnelMovement;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Rules;

public class PmBusinessRules
{
    private readonly IPmTypeService _pmTypeService;
    private readonly IStaffService _staffService;
    private readonly ITemporarayStaffService _temporarayStaffService;
    private readonly IPmService _pmService;
    private readonly IPersonnelService _personnelService;
    private readonly IHealthFacilityService _healthFacilityService;

    public PmBusinessRules(
        IPmTypeService pmTypeService, 
        IStaffService staffService, 
        ITemporarayStaffService temporarayStaffService,
        IPmService pmService,
        IPersonnelService personnelService,
        IHealthFacilityService healthFacilityService)
    {
        _pmTypeService = pmTypeService;
        _staffService = staffService;
        _temporarayStaffService = temporarayStaffService;
        _pmService = pmService;
        _personnelService = personnelService;
        _healthFacilityService = healthFacilityService;
    }

    public async Task<Result<string>> CheckRulesForCreateAsync(CreatePersonelMovementDto model)
    {
        var pmType = await _pmTypeService.GetAll().FirstOrDefaultAsync(t => t.Id == model.PmTypeId);
        
        if (pmType == null)
            return Result<string>.Fail("Hareket türü bulunamadı.");

        // Genel Kural: Aynı kişi, aynı kurumda, aynı hareket türü ile birden fazla aktif kayda sahip olamaz.
        var isDuplicateAtFacility = await _pmService.GetAll()
            .AnyAsync(pm => pm.PersonnelId == model.PersonnelId 
                         && pm.HealthFacilityId == model.HealthFacilityId
                         && pm.PmTypeId == model.PmTypeId
                         && (pm.Finish == null || pm.Finish >= DateTime.Now));

        if (isDuplicateAtFacility)
            return Result<string>.Fail("Bu personelin ilgili kurumda bu hareket türüyle devam eden aktif bir kaydı zaten mevcut.");

        // 1- IsUsingStaff
        if (pmType.IsUsingStaff)
        {
            var targetFacility = await _healthFacilityService.GetAll()
                .Include(hf => hf.HealthFacilityType)
                .FirstOrDefaultAsync(hf => hf.Id == model.HealthFacilityId);

            bool isTargetMh = targetFacility?.HealthFacilityType?.Code == "MH";

            if (!isTargetMh)
            {
                var staff = await _staffService.GetAll()
                    .FirstOrDefaultAsync(s => s.HealthFacilityId == model.HealthFacilityId && s.BranchId == model.BranchId);

                if (staff == null)
                    return Result<string>.Fail("İlgili tesis ve branş için kadro tanımlı değil.");

                var activeCount = await _pmService.GetAll()
                    .CountAsync(pm => pm.HealthFacilityId == model.HealthFacilityId 
                                   && pm.BranchId == model.BranchId 
                                   && pm.PmTypeId == model.PmTypeId
                                   && (pm.Finish == null || pm.Finish >= DateTime.Now));

                if (activeCount >= staff.Count)
                    return Result<string>.Fail($"Kadro yetersiz. (Kapasite: {staff.Count}, Mevcut: {activeCount})");
            }
        }

        // 2- IsBeforeStartStaff
        if (pmType.IsBeforeStartStaff)
        {
            var hasKadrolu = await _pmService.GetAll()
                .Include(pm => pm.PmType)
                .AnyAsync(pm => pm.PersonnelId == model.PersonnelId 
                             && pm.PmType != null 
                             && pm.PmType.Code == "KAD");

            if (!hasKadrolu)
                return Result<string>.Fail("Bu hareket türünü ekleyebilmek için personelin daha önce Kadrolu (KAD) olarak başlamış olması gerekmektedir.");
        }

        // 3- IsFaaliyet2Control
        if (pmType.IsFaaliyet2Control)
        {
            var hasTempStaff = await _temporarayStaffService.GetAll()
                .AnyAsync(ts => ts.HealthFacilityId == model.HealthFacilityId 
                             && ts.BranchId == model.BranchId 
                             && ts.PmTypeId == model.PmTypeId);

            if (!hasTempStaff)
                return Result<string>.Fail("Geçici kadroda bu tesis ve branş için ilgili hareket türüne ait kayıt bulunmamaktadır.");
        }

        // 5- StatusQuota
        if (pmType.StatusQuota > 0)
        {
            var activePersonelCount = await _pmService.GetAll()
                .CountAsync(pm => pm.PersonnelId == model.PersonnelId
                               && pm.PmTypeId == model.PmTypeId
                               && (pm.Finish == null || pm.Finish >= DateTime.Now));

            if (activePersonelCount >= pmType.StatusQuota)
                return Result<string>.Fail($"Bu hareket türü için personel kotası dolmuştur. (Kota: {pmType.StatusQuota}, Mevcut: {activePersonelCount})");
        }

        // 6- OHY24 Kuralı: İlgili branştaki OHY24 sayısı, hastanenin ilgili branştaki kadrosunun 1/3'ünü geçemez
        if (pmType.Code == "OHY24")
        {
            var staff = await _staffService.GetAll()
                .FirstOrDefaultAsync(s => s.HealthFacilityId == model.HealthFacilityId && s.BranchId == model.BranchId);

            if (staff == null)
                return Result<string>.Fail("İlgili tesis ve branş için kadro tanımlı değil (1/3 kuralı işletilemiyor).");

            var activeOhy24Count = await _pmService.GetAll()
                .CountAsync(pm => pm.HealthFacilityId == model.HealthFacilityId 
                               && pm.BranchId == model.BranchId 
                               && pm.PmTypeId == model.PmTypeId
                               && (pm.Finish == null || pm.Finish >= DateTime.Now));

            if(staff.Count>0 && staff.Count<3) staff.Count= 3; // Kadro 3'ten az ise, 1/3 kuralı işletilemeyeceği için kadroyu 3 olarak varsayıyoruz.

            int limit = staff.Count / 3;

            if (activeOhy24Count >= limit)
                return Result<string>.Fail($"Bu branş için OHY24 hareket türü sayısı, kadronun 1/3'ünü geçemez. (Kadro: {staff.Count}, İzin Verilen: {limit}, Mevcut: {activeOhy24Count})");
        }

        // 7- OHY60 Kuralı: Eğer hareket türü OHY60 ise personelin yaşı 60 ve üstü olmalıdır.
        if (pmType.Code == "OHY24-60")
        {
            var personnel = await _personnelService.GetAll()
                .FirstOrDefaultAsync(p => p.Id == model.PersonnelId);

            if (personnel == null)
                return Result<string>.Fail("Personel bulunamadı.");

            if (!personnel.BirthDate.HasValue)
                return Result<string>.Fail("Personelin doğum tarihi bilgisi bulunmuyor. Lütfen önce doğum tarihini güncelleyiniz.");

            var today = DateTime.Today;
            var age = today.Year - personnel.BirthDate.Value.Year;
            if (personnel.BirthDate.Value.Date > today.AddYears(-age)) age--;

            if (age < 60)
                return Result<string>.Fail($"OHY60 hareket türü için personelin yaşı 60 ve üzeri olmalıdır. (Mevcut yaş: {age})");
        }

        // 8- OHY24-MUH Kuralı: Personel daha önce MH kodlu ve açılış tarihi 01.07.2023 öncesi olan bir kurumda başlamış olmalı.
        if (pmType.Code == "OHY24-MUH")
        {
            var hasStartedInMHBefore = await _pmService.GetAll()
                .Include(pm => pm.HealthFacility)
                .ThenInclude(hf => hf.HealthFacilityType)
                .AnyAsync(pm => pm.PersonnelId == model.PersonnelId
                             && pm.HealthFacility != null
                             && pm.HealthFacility.HealthFacilityType != null
                             && pm.HealthFacility.HealthFacilityType.Code == "MH"
                             && pm.HealthFacility.OpeningDate != null
                             && pm.HealthFacility.OpeningDate < new DateTime(2023, 7, 1));

            if (!hasStartedInMHBefore)
                return Result<string>.Fail("OHY24-MUH hareket türü eklenebilmesi için personelin daha önce açılış tarihi 01.07.2023'ten önce olan ve türü Muayenehane (MH) olan bir kurumda başlamış olması gerekmektedir.");
        }

        return Result<string>.Ok("Kurallar geçerli");
    }
}
