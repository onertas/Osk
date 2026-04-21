using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

    [Route("api/[controller]/[action]")]
    [ApiController]

    public class HealthFacilityTypeController : ControllerBase
    {


    private readonly IUnitOfWork _unitOfWork;
    private readonly IHealthFacilityTypeService _healthFacilityTypeService;
    public HealthFacilityTypeController(IUnitOfWork unitOfWork, IHealthFacilityTypeService healthFacilityTypeService)
    {
        _unitOfWork = unitOfWork;
        _healthFacilityTypeService = healthFacilityTypeService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetHealthFacilityTypes()
    {
        var list = await _healthFacilityTypeService.GetAll().ToListAsync();


        var res=Result<List<HealthFacilityType>>.Ok(list);
        return Ok(res);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Add(HealthFacilityType model)
    {
        await _healthFacilityTypeService.AddAsync(model);
        await _unitOfWork.SaveChangesAsync();

     
        return Ok(Result.Ok("Eklendi"));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Update(HealthFacilityType model)
    {
        var entity = await _healthFacilityTypeService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        entity.Name = model.Name;
        entity.Code = model.Code;
        
        _healthFacilityTypeService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _healthFacilityTypeService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _healthFacilityTypeService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}

