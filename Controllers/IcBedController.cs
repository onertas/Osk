using AutoMapper;
using GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.Beds;
using OskApi.Entities.Beds;
using OskApi.Services.Abstract;
using OskApi.Shared.Result;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IcBedController : ControllerBase
    {
        private readonly IIcBedService _icBedService;
        private readonly IIcBedNameService _icBedNameService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IcBedController(IIcBedService icBedService, IIcBedNameService icBedNameService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _icBedService = icBedService;
            _icBedNameService = icBedNameService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateIcBedDto model)
        {
            var entity = _mapper.Map<IcBed>(model);
            
            // SmartEnum mapping from int values
            entity.IcBedRegLevel = IcBedRegLevel.FromValue(model.IcBedRegLevel);
            entity.IcBedRegType = IcBedRegType.FromValue(model.IcBedRegType);

            try
            {
                await _icBedService.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return Ok(Result.Ok("Eklendi"));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateIcBedDto model)
        {
            var entity = await _icBedService.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

            _mapper.Map(model, entity);
            
            entity.IcBedRegLevel = IcBedRegLevel.FromValue(model.IcBedRegLevel);
            entity.IcBedRegType = IcBedRegType.FromValue(model.IcBedRegType);

            try
            {
                _icBedService.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return Ok(Result.Ok("Güncellendi"));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var entity = await _icBedService.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound(Result.Fail("Kayıt bulunamadı"));

            try
            {
                _icBedService.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return Ok(Result.Ok("Silindi"));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Fail(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByHfId(Guid healthFacilityId)
        {
            var list = await _icBedService.GetAll()
                .Include(x => x.IcBedName)
                .Where(x => x.HealthFacilityId == healthFacilityId)
                .ToListAsync();

            var mappedList = _mapper.Map<List<ListIcBedDto>>(list);
            return Ok(Result<List<ListIcBedDto>>.Ok(mappedList));
        }

        [HttpGet]
        public IActionResult GetIcBedTypes()
        {
            var types = IcBedType.List.Select(x => new 
            { 
                value = x.Value, 
                name = x.Name, 
                description = x.Description 
            }).ToList();
            return Ok(Result<object>.Ok(types));
        }

        [HttpGet]
        public async Task<IActionResult> GetIcBedNames(int? typeValue)
        {
            var query = _icBedNameService.GetAll();
            
            if (typeValue.HasValue)
            {
                var bedType = IcBedType.FromValue(typeValue.Value);
                query = query.Where(x => x.IcBedType == bedType);
            }

            var list = await query.ToListAsync();
            var mappedList = list.Select(x => new 
            { 
                id = x.Id, 
                name = x.Name, 
                icBedType = x.IcBedType.Value 
            }).ToList();
            
            return Ok(Result<object>.Ok(mappedList));
        }

        [HttpGet]
        public IActionResult GetIcBedRegLevels()
        {
            var levels = IcBedRegLevel.List.Select(x => new 
            { 
                value = x.Value, 
                name = x.Name, 
                description = x.Description 
            }).ToList();
            return Ok(Result<object>.Ok(levels));
        }

        [HttpGet]
        public IActionResult GetIcBedRegTypes()
        {
            var types = IcBedRegType.List.Select(x => new 
            { 
                value = x.Value, 
                name = x.Name, 
                description = x.Description 
            }).ToList();
            return Ok(Result<object>.Ok(types));
        }
    }
}
