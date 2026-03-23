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
}
