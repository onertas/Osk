using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.Staff;
using OskApi.Entities.Staff;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TemporarayStaffController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITemporarayStaffService _temporarayStaffService;
    private readonly IMapper _mapper;

    public TemporarayStaffController(IUnitOfWork unitOfWork, ITemporarayStaffService temporarayStaffService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _temporarayStaffService = temporarayStaffService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateTemporarayStaffDto model)
    {
        var exists = await _temporarayStaffService.GetAll().AnyAsync(x => x.HealthFacilityId == model.HealthFacilityId && x.BranchId == model.BranchId && x.PmTypeId == model.PmTypeId);
        if (exists) return BadRequest(Result.Fail("Bu kurum için aynı tipte bu kadro zaten eklenmiş."));

        var entity = _mapper.Map<TemporarayStaff>(model);
        await _temporarayStaffService.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Eklendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateTemporarayStaffDto model)
    {
        var entity = await _temporarayStaffService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

        _mapper.Map(model, entity);
        _temporarayStaffService.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Güncellendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _temporarayStaffService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

        _temporarayStaffService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Silindi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _temporarayStaffService.GetAll().Where(i => i.IsDeteled == false)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .Include(i => i.PmType)
            .ToListAsync();

        var mappedList = _mapper.Map<List<ListTemporarayStaffDto>>(list);
        return Ok(Result<List<ListTemporarayStaffDto>>.Ok(mappedList));
    }

    [HttpGet]
    public async Task<IActionResult> GetByHealthFacilityId([FromQuery] Guid id)
    {
        var list = await _temporarayStaffService.GetAll().Where(i => i.IsDeteled == false && i.HealthFacilityId == id)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .Include(i => i.PmType)
            .ToListAsync();

        var mappedList = _mapper.Map<List<ListTemporarayStaffDto>>(list);
        return Ok(Result<List<ListTemporarayStaffDto>>.Ok(mappedList));
    }
}
