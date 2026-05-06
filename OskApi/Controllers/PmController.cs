using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.PersonnelMovement;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Authorize]
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

    [HttpGet]
    public async Task<IActionResult> GetByPersonnelId(Guid personnelId)
    {
        var list = await _pmService.GetAll()
            .Include(i => i.PmType)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .Include(i => i.Personnel)
            .Where(i => i.PersonnelId == personnelId)
            .ToListAsync();
            
        var mappedList = _mapper.Map<List<ListPersonelMovementDto>>(list);
        
        var result = Result<List<ListPersonelMovementDto>>.Ok(mappedList);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByHfId(Guid healthFacilityId, bool showSeparated = false)
    {
        var query = _pmService.GetAll()
            .Include(i => i.PmType)
            .Include(i => i.Branch)
            .Include(i => i.HealthFacility)
            .Include(i => i.Personnel)
            .Where(i => i.HealthFacilityId == healthFacilityId);

        if (!showSeparated)
        {
            var now = DateTime.Now;
            query = query.Where(i => i.Finish == null);
        }

        var list = await query.ToListAsync();
            
        var mappedList = _mapper.Map<List<ListPersonelMovementDto>>(list);
        
        var result = Result<List<ListPersonelMovementDto>>.Ok(mappedList);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(UpdatePersonelMovementDto model)
    {
        model.Start = model.Start.ToLocalTime();
        if (model.Finish.HasValue) model.Finish = model.Finish.Value.ToLocalTime();
        if (model.ContractStart.HasValue) model.ContractStart = model.ContractStart.Value.ToLocalTime();
        if (model.ContractFinish.HasValue) model.ContractFinish = model.ContractFinish.Value.ToLocalTime();

        var entity = await _pmService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _mapper.Map(model, entity);
        
        _pmService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [Authorize(Roles = "Admin")]
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
    [HttpGet]
    public async Task<IActionResult> GetReport(
        int page = 1, 
        int pageSize = 10, 
        Guid? pmTypeId = null, 
        Guid? healthFacilityId = null, 
        Guid? healthFacilityTypeId = null,
        bool? isActive = null,
        Guid? titleId = null,
        Guid? branchId = null,
        string? search = null)
    {
        var query = _pmService.GetAll()
            .Include(i => i.PmType)
            .Include(i => i.Branch).ThenInclude(b => b!.Title)
            .Include(i => i.HealthFacility).ThenInclude(hf => hf!.HealthFacilityType)
            .Include(i => i.Personnel)
            .AsQueryable();

        if (pmTypeId.HasValue)
            query = query.Where(w => w.PmTypeId == pmTypeId.Value);

        if (healthFacilityId.HasValue)
            query = query.Where(w => w.HealthFacilityId == healthFacilityId.Value);

        if (healthFacilityTypeId.HasValue)
            query = query.Where(w => w.HealthFacility!.HealthFacilityTypeId == healthFacilityTypeId.Value);

        if (isActive.HasValue)
        {
            if (isActive.Value)
                query = query.Where(w => w.Finish == null);
            else
                query = query.Where(w => w.Finish != null);
        }

        if (titleId.HasValue)
            query = query.Where(w => w.Branch!.TitleId == titleId.Value);

        if (branchId.HasValue)
            query = query.Where(w => w.BranchId == branchId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(w => 
                w.Personnel!.FirstName.ToLower().Contains(s) || 
                w.Personnel!.LastName.ToLower().Contains(s) ||
                (w.Personnel!.IdentityNumber != null && w.Personnel!.IdentityNumber.Contains(s))
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(o => o.Start)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var mappedItems = _mapper.Map<List<ListPersonelMovementDto>>(items);

        return Ok(Result<object>.Ok(new
        {
            items = mappedItems,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        }, "Rapor Listelendi"));
    }
}
