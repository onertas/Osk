using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.Staff;
using OskApi.Entities.Staff;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStaffService _staffService;
    private readonly IMapper _mapper;

    public StaffController(IUnitOfWork unitOfWork, IStaffService staffService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _staffService = staffService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(CreateStaffDto model)
    {
     

        var entity = _mapper.Map<Staff>(model);
        
        var lastStaff = await _staffService.GetAll()
            .OrderByDescending(s => s.StaffNo)
            .FirstOrDefaultAsync();
        
        entity.StaffNo = (lastStaff?.StaffNo ?? 0) + 1;

        await _staffService.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Eklendi"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Update(UpdateStaffDto model)
    {
        var entity = await _staffService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

        _mapper.Map(model, entity);
        _staffService.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Güncellendi"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _staffService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

        _staffService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return Ok(Result.Ok("Silindi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _staffService.GetAll().Where(i=>i.IsDeteled==false)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .ToListAsync();

        var mappedList = _mapper.Map<List<ListStaffDto>>(list);
        return Ok(Result<List<ListStaffDto>>.Ok(mappedList));
    }

    [HttpGet]
    public async Task<IActionResult> GetByHealthFacilityId([FromQuery] Guid id)
    {
        var list = await _staffService.GetAll().Where(i => i.IsDeteled == false && i.HealthFacilityId == id)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .ToListAsync();

        var mappedList = _mapper.Map<List<ListStaffDto>>(list);
        return Ok(Result<List<ListStaffDto>>.Ok(mappedList));
    }
}
