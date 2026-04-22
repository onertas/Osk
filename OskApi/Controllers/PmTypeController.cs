using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.PmType;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PmTypeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPmTypeService _pmTypeService;
    private readonly IMapper _mapper;

    public PmTypeController(IUnitOfWork unitOfWork, IPmTypeService pmTypeService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _pmTypeService = pmTypeService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreatePmTypeDto model)
    {
        var entity = _mapper.Map<PmType>(model);
        
        try
        {
            await _pmTypeService.AddAsync(entity);
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
        var list = await _pmTypeService.GetAll().ToListAsync();
        var mappedList = _mapper.Map<List<ListPmTypeDto>>(list);
        
        var result = Result<List<ListPmTypeDto>>.Ok(mappedList);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePmTypeDto model)
    {
        var entity = await _pmTypeService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _mapper.Map(model, entity);
        
        _pmTypeService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _pmTypeService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _pmTypeService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}
