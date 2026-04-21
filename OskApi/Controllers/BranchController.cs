using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class BranchController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBranchService _branchService;

    public BranchController(IUnitOfWork unitOfWork, IBranchService branchService)
    {
        _unitOfWork = unitOfWork;
        _branchService = branchService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Add(Branch model)
    {
        await _branchService.AddAsync(model);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Eklendi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _branchService.GetAll().Include(i => i.Title).ToListAsync();

        var res = Result<List<Branch>>.Ok(list, "Veri Listelendi");
        return Ok(res);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Branch model)
    {
        var entity = await _branchService.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        entity.Name = model.Name;
        entity.TitleId = model.TitleId;
        entity.BranchTypeId = model.BranchTypeId;
        
        _branchService.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Güncellendi"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var entity = await _branchService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return NotFound(Result.Fail("Kayıt bulunamadı"));

        _branchService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }
}
