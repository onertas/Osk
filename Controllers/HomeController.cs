using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.User;
using OskApi.Entities;
using OskApi.Entities.User;
using OskApi.Services;
using OskApi.Services.Jwt;
using OskApi.Shared.Result;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(IUnitOfWork unitOfWork, IProductService productService, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _userManager = userManager;
            _tokenService = tokenService;
        }


        [HttpPost]
        public async Task<IActionResult> Add(Product model)
        {
         
            await  _productService.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();


            var res = Result.Ok("İşlem Başarılı");

            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _productService.GetAll().ToListAsync();


            var res=Result<List<Product>>.Ok(list,"Veri Eklendi");

            return Ok(res);
        }

    }
}
