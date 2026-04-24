using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.PersonnelMovement;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PmController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPmService _pmService;
    private readonly IPmTypeService _pmTypeService;
    private readonly IStaffService _staffService;
    private readonly IMapper _mapper;
    private readonly OskApi.Rules.PmBusinessRules _pmBusinessRules;

    public PmController(
        IUnitOfWork unitOfWork, 
        IPmService pmService, 
        IPmTypeService pmTypeService,
        IStaffService staffService,
        IMapper mapper,
        OskApi.Rules.PmBusinessRules pmBusinessRules)
    {
        _unitOfWork = unitOfWork;
        _pmService = pmService;
        _pmTypeService = pmTypeService;
        _staffService = staffService;
        _mapper = mapper;
        _pmBusinessRules = pmBusinessRules;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreatePersonelMovementDto model)
    {
        // İş Kurallarını Kontrol Et
        var ruleResult = await _pmBusinessRules.CheckRulesForCreateAsync(model);
        if (!ruleResult.Success)
        {
            return BadRequest(ruleResult);
        }

        var entity = _mapper.Map<PersonnelMovement>(model);
        
        try
        {
            await _pmService.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(Result.Fail(e.Message));
        }

        return Ok(Result.Ok("Eklendi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _pmService.GetAll()
            .Include(i => i.PmType)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .Include(i => i.Personnel)
            .ToListAsync();
            
        var mappedList = _mapper.Map<List<ListPersonelMovementDto>>(list);
        
        var result = Result<List<ListPersonelMovementDto>>.Ok(mappedList);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePersonelMovementDto model)
    {
        model.Start = model.Start.ToLocalTime();

        var entity = await _pmService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _mapper.Map(model, entity);
        
        _pmService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _pmService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _pmService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}
