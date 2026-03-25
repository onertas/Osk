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

        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(IUnitOfWork unitOfWork,  UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
       
            _userManager = userManager;
            _tokenService = tokenService;
        }


        


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}
