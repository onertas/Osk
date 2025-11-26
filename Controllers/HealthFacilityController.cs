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

    public class HealthFacilityController : ControllerBase
    {


    private readonly IUnitOfWork _unitOfWork;
    private readonly IHealthFacilityService _healthFacilityService;
    public HealthFacilityController(IUnitOfWork unitOfWork, IHealthFacilityService healthFacilityService)
    {
        _unitOfWork = unitOfWork;
        _healthFacilityService = healthFacilityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealthFacilities(Guid Id)
    {
        var list = await _healthFacilityService.GetAll().Where(x=>x.HealthFacilityTypeId==Id).ToListAsync();


        var res=Result<List<HealthFacility>>.Ok(list,"Veri Eklendi");
        return Ok(res);
    }

}

