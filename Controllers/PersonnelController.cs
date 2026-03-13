using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OskApi.Dtos.Personnel;
using OskApi.Entities.Personnel;
using OskApi.Services.Abstract;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonnelService _personnelService;
        public PersonnelController(IUnitOfWork unitOfWork, IPersonnelService personnelService)
        {
            _unitOfWork = unitOfWork;
            _personnelService = personnelService;
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreatePersonnelDto model)
        {
            var entity = new Personnel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
            };


            await _personnelService.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(Results.Ok("Eklendi"));
        }


    }
}
