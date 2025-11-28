using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.HealthFacilities;
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

    [HttpGet]
    public async Task<IActionResult> GetHealthFacilityTypes()
    {
        var list = await _healthFacilityTypeService.GetAll().ToListAsync();


        var res=Result<List<HealthFacilityType>>.Ok(list);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Add(HealthFacilityType model)
    {
        await _healthFacilityTypeService.AddAsync(model);
        await _unitOfWork.SaveChangesAsync();

     
        return Ok(Result.Ok("Eklendi"));
    }

}

