using AutoMapper;
using GenericRepository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OskApi.Dtos.Personnel;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PersonnelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonnelService _personnelService;
    private readonly IMapper _mapper;
    public PersonnelController(IUnitOfWork unitOfWork, IPersonnelService personnelService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _personnelService = personnelService;
        _mapper = mapper;
    }


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

        };

        try
        {
            await _personnelService.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {

            Console.WriteLine(e);
        }



        return Ok(Results.Ok("Eklendi"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _personnelService.GetAll().Include(i => i.PersonnelBranches!)
            .ThenInclude(i => i.Branch!).ThenInclude(i => i.Title).ToListAsync();

        var mappedlist = _mapper.Map<List<ListPersonnelDto>>(list);

        var result = Result<List<ListPersonnelDto>>.Ok(mappedlist);

        return Ok(result);


    }
}



