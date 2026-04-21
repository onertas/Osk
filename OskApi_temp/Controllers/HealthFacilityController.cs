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

    public HealthFacilityController(IUnitOfWork unitOfWork, IHealthFacilityService healthFacilityService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _healthFacilityService = healthFacilityService;
        _mapper = mapper;
    }

    [AllowAnonymous]
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

        if (list == null || list.Count == 0)
            return NotFound(Result.Fail("Veri bulunamadı"));

        var listdto = list.Select(x => new HealthFacilityListDto
        {
            Id = x.Id,
            Name = x.Name,
            TypeName = x.HealthFacilityType!.Name,
        }).ToList();

        return Ok(Result<List<HealthFacilityListDto>>.Ok(listdto, "Veri Listelendi"));
    }

    /// <summary>
    /// Tüm sağlık tesislerini sayfalı ve arama destekli listeler (Yönetim ekranı için)
    /// GET /api/HealthFacility/GetAllPaged?page=1&pageSize=10&search=xxx
    /// </summary>
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

        var items = await query
            .OrderBy(o => o.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new HfManagementListDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                TaxNumber = x.TaxNumber,
                CorporationName = x.CorporationName,
                HealthFacilityTypeId = x.HealthFacilityTypeId,
                TypeName = x.HealthFacilityType != null ? x.HealthFacilityType.Name : ""
            })
            .ToListAsync();

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

        _healthFacilityService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    /// <summary>
    /// Sağlık tesisi sil
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _healthFacilityService.GetAll()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _healthFacilityService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}


