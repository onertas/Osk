using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.HealthFacilities;
using OskApi.Entities.HealthFacilities;
using OskApi.Services.HealthFacilities;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetHealthFacilities(Guid Id)
    {
        var list = await _healthFacilityService.GetAll().Include(i=>i.HealthFacilityType).Where(x=>x.HealthFacilityTypeId==Id).ToListAsync();

      var listdto=  list.Select(x => new HealthFacilityListDto
        {
            Id = x.Id,
            Name = x.Name,
            TypeName = x.HealthFacilityType!.Name,

      }).ToList();

        var res=Result<List<HealthFacilityListDto>>.Ok(listdto,"Veri Eklendi");
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateHealthFacilityDto model)
    {

        var entity=_mapper.Map<HealthFacility>(model);


        await _healthFacilityService.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
     
        return Ok(Result.Ok("Eklendi"));
    }   

}

