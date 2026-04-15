using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.HealthFacilities;
using OskApi.Entities.HealthFacilities;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;
namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]

public class TitleController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITitleService _titleService;

    public TitleController(IUnitOfWork unitOfWork, IMapper mapper, ITitleService titleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _titleService = titleService;
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Add(Title model)
    {

        await _titleService.AddAsync(model);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Eklendi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
         var list = await _titleService.GetAll().ToListAsync();

      ;

        var res = Result<List<Title>>.Ok(list, "Veri Listelendi");
        return Ok(res);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Title model)
    {
        var entity = await _titleService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        entity.Name = model.Name;
        
        _titleService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _titleService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _titleService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}

