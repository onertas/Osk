using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OskApi.Dtos.Personnel;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class PersonnelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonnelService _personnelService;
    private readonly IMapper _mapper;
    private readonly OskApi.Data.MyDbContext _context;

    public PersonnelController(IUnitOfWork unitOfWork, IPersonnelService personnelService, IMapper mapper, OskApi.Data.MyDbContext context)
    {
        _unitOfWork = unitOfWork;
        _personnelService = personnelService;
        _mapper = mapper;
        _context = context;
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(CreatePersonnelDto model)
    {
        var entity = new Personnel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PersonnelBranches = model.PersonnelBranches?.Select(i => new PersonnelBranch
            {
                BranchId = i
            }).ToList(),
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IdentityNumber = model.IdentityNumber,
            IsBanned = model.IsBanned,
            BirthDate = model.BirthDate
        };

        try
        {
            await _personnelService.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(Result.Ok("Eklendi"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(Result.Fail(e.Message));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Update(UpdatePersonnelDto model)
    {
        var entity = await _personnelService.GetAll()
            .Include(i => i.PersonnelBranches)
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (entity == null) return NotFound(Result.Fail("Personel bulunamadı"));

        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.PhoneNumber = model.PhoneNumber;
        entity.IdentityNumber = model.IdentityNumber;
        entity.IsBanned = model.IsBanned;
        entity.BirthDate = model.BirthDate;

        // Update Branches using DbContext to remove orphaned records
        if (entity.PersonnelBranches != null && entity.PersonnelBranches.Any())
        {
            _context.PersonnelBranches.RemoveRange(entity.PersonnelBranches);
            entity.PersonnelBranches.Clear();
        }
        else if (entity.PersonnelBranches == null)
        {
            entity.PersonnelBranches = new List<PersonnelBranch>();
        }

        if (model.PersonnelBranches != null)
        {
            foreach (var branchId in model.PersonnelBranches)
            {
                entity.PersonnelBranches.Add(new PersonnelBranch { BranchId = branchId, PersonnelId = entity.Id });
            }
        }

        try
        {
            _personnelService.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(Result.Ok("Güncellendi"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(Result.Fail(e.Message));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _personnelService.GetAll().FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null) return NotFound(Result.Fail("Personel bulunamadı"));

        // Personelin hareket kaydı varsa silinemez
        var hasMovements = await _context.PersonnelMovements.AnyAsync(m => m.PersonnelId == id);
        if (hasMovements)
        {
            return BadRequest(Result.Fail("Bu personelin hareket kaydı bulunduğu için silinemez. Önce hareketlerini silmelisiniz."));
        }

        _personnelService.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(Result.Ok("Silindi"));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _personnelService.GetAll().Include(i => i.PersonnelBranches!)
            .ThenInclude(i => i.Branch!).ThenInclude(i => i.Title).ToListAsync();

        var mappedlist = _mapper.Map<List<ListPersonnelDto>>(list);

        var result = Result<List<ListPersonnelDto>>.Ok(mappedlist);

        return Ok(result);


    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public async Task<IActionResult> GetAllPaged(int page = 1, int pageSize = 10, string? search = null)
    {
        var query = _personnelService.GetAll()
            .Include(i => i.PersonnelBranches!)
                .ThenInclude(i => i.Branch!)
                    .ThenInclude(b => b.Title)
            .AsQueryable();

     

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(w =>
                w.FirstName.ToLower().Contains(s) ||
                w.LastName.ToLower().Contains(s) ||
                (w.IdentityNumber != null && w.IdentityNumber.Contains(s)) ||
                (w.Email != null && w.Email.ToLower().Contains(s))
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(o => o.FirstName)
            .ThenBy(o => o.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var mappedItems = _mapper.Map<List<ListPersonnelDto>>(items);

        return Ok(Result<object>.Ok(new
        {
            items = mappedItems,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        }, "Listelendi"));
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? query)
    {
        var list = await _personnelService.Search(query).ToListAsync();

        var mappedlist = _mapper.Map<List<ListPersonnelDto>>(list);

        var result = Result<List<ListPersonnelDto>>.Ok(mappedlist);

        return Ok(result);
    }
}



