using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.User;
using OskApi.Entities.User;
using OskApi.Services;
using OskApi.Services.Jwt;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
      
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthController(IUnitOfWork unitOfWork,  UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
      
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {

            var anyuser = await _userManager.Users.CountAsync();
            
            if(anyuser==0)
            {
               var r= await _userManager.CreateAsync(new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                }, "Admin123*");
            }

            await _userManager.CreateAsync(new AppUser
            {
                UserName = dto.UserName,
                Email = dto.UserName
            }, dto.Password);




            return Ok(new { msg = "Kullanıcı Oluşturuldu" });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) return Unauthorized("Kullanıcı bulunamadı");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
           // result = true;
            if (!result) return Unauthorized("Hatalı şifre");

            var accessToken = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            // 🍪 Cookie olarak token yaz

            CreateCookie("accessToken", accessToken, DateTime.UtcNow.AddMinutes(60));
            CreateCookie("refreshToken", refreshToken, DateTime.UtcNow.AddDays(2));

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            //var accessToken = Request.Cookies["accessToken"];
            //var refreshToken = Request.Cookies["refreshToken"];
            //if (accessToken == null || refreshToken == null)
            //    return Unauthorized("Token bulunamadı");
            //var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            //if (principal == null)
            //    return Unauthorized("Geçersiz token");
            //var username = principal.Identity?.Name;
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return Unauthorized("Refresh token bulunamadı");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token geçersiz");

          
            var newAccessToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            // 🍪 Cookie olarak token yaz

            CreateCookie("accessToken", newAccessToken, DateTime.UtcNow.AddSeconds(60));
            CreateCookie("refreshToken", newRefreshToken, DateTime.UtcNow.AddDays(2));

            //Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.UtcNow.AddMinutes(1)
            //});
            //Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.UtcNow.AddDays(7)
            //});
            return Ok(new { message = "Token yenilendi" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı al
            var username = User.Identity?.Name;
            if (username == null)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                // Refresh token sıfırla
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(-1);
                await _userManager.UpdateAsync(user);
            }


            // Cookie'leri SİL - boş string veya Delete kullan
            Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // production: true
                SameSite = SameSiteMode.None,
               
            });

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // production: true
                SameSite = SameSiteMode.None,
                
            });

            return Ok(new { message = "Çıkış yapıldı" });
        }

        [HttpGet]
        public IActionResult IsAuthenticated()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Ok();
            return Unauthorized();
        }
        private void CreateCookie(string name,string token,DateTime express )
        {
            Response.Cookies.Append(name, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = express
            });

        }

    }
}
