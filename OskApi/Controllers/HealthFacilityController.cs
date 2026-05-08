using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.HealthFacilities;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class HealthFacilityController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHealthFacilityService _healthFacilityService;
    private readonly IPmService _pmService;

    public HealthFacilityController(IUnitOfWork unitOfWork, IHealthFacilityService healthFacilityService, IMapper mapper, IPmService pmService)
    {
        _unitOfWork = unitOfWork;
        _healthFacilityService = healthFacilityService;
        _mapper = mapper;
        _pmService = pmService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(CreateHealthFacilityDto model)
    {
        var entity = _mapper.Map<HealthFacility>(model);
        await _healthFacilityService.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Eklendi"));
    }

    /// <summary>
    /// Belirli bir tipe göre HF listele (mevcut endpoint, geriye dönük uyumluluk için korundu)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetHealthFacilities(string code)
    {
        var list = await _healthFacilityService.GetAll()
            .Where(w => w.HealthFacilityType!.Code == code)
            .Include(i => i.HealthFacilityType)
            .ToListAsync();


        var listdto = list.Select(x => new HealthFacilityListDto
        {
            Id = x.Id,
            Name = x.Name,
            TypeName = x.HealthFacilityType!.Name,
        }).ToList();

        return Ok(Result<List<HealthFacilityListDto>>.Ok(listdto, "Veri Listelendi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _healthFacilityService.GetAll()
            .Include(x => x.HealthFacilityType)
            .Select(x => new HealthFacilityListDto
            {
                Id = x.Id,
                Name = x.Name,
                TypeName = x.HealthFacilityType != null ? x.HealthFacilityType.Name : ""
            }).ToListAsync();

        return Ok(Result<List<HealthFacilityListDto>>.Ok(list, "Tüm Veriler Listelendi"));
    }

    /// <summary>
    /// Belirli bir sağlık tesisini ID ile getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await _healthFacilityService.GetAll()
            .Include(i => i.HealthFacilityType)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        var dto = new HfManagementListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            TaxNumber = entity.TaxNumber,
            CorporationName = entity.CorporationName,
            ObservationBedCount = entity.ObservationBedCount,
            TotalBedCount = entity.TotalBedCount,
            HealthFacilityTypeId = entity.HealthFacilityTypeId,
            TypeName = entity.HealthFacilityType?.Name ?? "",
            ShowBed = entity.HealthFacilityType?.ShowBed ?? false,
            ShowDevice = entity.HealthFacilityType?.ShowDevice ?? false,
            ShowStaff = entity.HealthFacilityType?.ShowStaff ?? false,
            ShowTempStaff = entity.HealthFacilityType?.ShowTempStaff ?? false,
            ShowPm = entity.HealthFacilityType?.ShowPm ?? false,
            UpperHealthFacilityId = entity.UpperHealthFacilityId,
            HfStatus = entity.HfStatus.Value,
            HfStatusName = entity.HfStatus.Name
        };

        return Ok(Result<HfManagementListDto>.Ok(dto, "Veri getirildi"));
    }

    /// <summary>
    /// Tüm sağlık tesislerini sayfalı ve arama destekli listeler (Yönetim ekranı için)
    /// GET /api/HealthFacility/GetAllPaged?page=1&pageSize=10&search=xxx
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllPaged(int page = 1, int pageSize = 10, string? search = null)
    {
        var query = _healthFacilityService.GetAll()
            .Include(i => i.HealthFacilityType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(w =>
                w.Name.ToLower().Contains(s) ||
                (w.CorporationName != null && w.CorporationName.ToLower().Contains(s)) ||
                (w.PhoneNumber != null && w.PhoneNumber.Contains(s)) ||
                (w.TaxNumber != null && w.TaxNumber.Contains(s)) ||
                (w.HealthFacilityType != null && w.HealthFacilityType.Name.ToLower().Contains(s))
            );
        }

        var totalCount = await query.CountAsync();

        var itemsList = await query
            .OrderBy(o => o.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var items = itemsList.Select(x => new HfManagementListDto
        {
            Id = x.Id,
            Name = x.Name,
            Address = x.Address,
            PhoneNumber = x.PhoneNumber,
            Email = x.Email,
            TaxNumber = x.TaxNumber,
            CorporationName = x.CorporationName,
            ObservationBedCount = x.ObservationBedCount,
            TotalBedCount = x.TotalBedCount,
            HealthFacilityTypeId = x.HealthFacilityTypeId,
            TypeName = x.HealthFacilityType?.Name ?? "",
            ShowBed = x.HealthFacilityType?.ShowBed ?? false,
            ShowDevice = x.HealthFacilityType?.ShowDevice ?? false,
            ShowStaff = x.HealthFacilityType?.ShowStaff ?? false,
            ShowTempStaff = x.HealthFacilityType?.ShowTempStaff ?? false,
            ShowPm = x.HealthFacilityType?.ShowPm ?? false,
            UpperHealthFacilityId = x.UpperHealthFacilityId,
            HfStatus = x.HfStatus.Value,
            HfStatusName = x.HfStatus.Name
        }).ToList();

        return Ok(Result<object>.Ok(new
        {
            items,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        }, "Listelendi"));
    }

    /// <summary>
    /// Sağlık tesisi güncelle
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Update(UpdateHealthFacilityDto model)
    {
        var entity = await _healthFacilityService.GetAll()
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        entity.Name = model.Name;
        entity.HealthFacilityTypeId = model.HealthFacilityTypeId;
        entity.Address = model.Address;
        entity.PhoneNumber = model.PhoneNumber;
        entity.Email = model.Email;
        entity.TaxNumber = model.TaxNumber;
        entity.CorporationName = model.CorporationName;
        entity.ObservationBedCount = model.ObservationBedCount;
        entity.TotalBedCount = model.TotalBedCount;
        entity.UpperHealthFacilityId = model.UpperHealthFacilityId;
        entity.HfStatus = HfStatus.FromValue(model.HfStatus);

        _healthFacilityService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    /// <summary>
    /// Sağlık tesisi sil
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _healthFacilityService.GetAll()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        var hasActiveMovements = await _pmService.GetAll()
            .AnyAsync(pm => pm.HealthFacilityId == id && (pm.Finish == null || pm.Finish >= DateTime.Now));

        if (hasActiveMovements)
            return BadRequest(Result.Fail("Bu kuruluşa ait aktif personel hareketleri bulunduğu için silinemez. Lütfen önce aktif kayıtları sonlandırın."));

        _healthFacilityService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetByUpperId(Guid upperId)
    {
        var list = await _healthFacilityService.GetAll()
            .Where(w => w.UpperHealthFacilityId == upperId)
            .Select(x => new HealthFacilityListDto
            {
                Id = x.Id,
                Name = x.Name,
                TypeName = x.HealthFacilityType != null ? x.HealthFacilityType.Name : ""
            }).ToListAsync();

        return Ok(Result<List<HealthFacilityListDto>>.Ok(list, "Alt Kurumlar Listelendi"));
    }
}


