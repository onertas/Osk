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
    private readonly IMapper _mapper;

    public PmController(IUnitOfWork unitOfWork, IPmService pmService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _pmService = pmService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreatePersonelMovementDto model)
    {
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
