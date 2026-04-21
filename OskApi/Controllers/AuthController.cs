using GenericRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.User;
using OskApi.Entities.User;
using OskApi.Services.Jwt;
using OskApi.Shared.Result;
using System.Security.Claims;

namespace OskApi.Controllers;

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
        CreateCookie("refreshToken", refreshToken, DateTime.UtcNow.AddDays(7)); // DB ile eşit

        return Ok(user);
    }


    [HttpPost]
    public async Task<IActionResult> RefreshToken()
    {
       
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
            return Unauthorized("Kullanıcı oturumu bulunamadı. Lütfen tekrar giriş yapın.");

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return Unauthorized("Kullanıcı oturumunun süresi dolmuş. Lütfen tekrar giriş yapın.");

      
        var newAccessToken = _tokenService.CreateToken(user);
        var newRefreshToken = _tokenService.CreateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // ✅ Süreyi de uzat
        await _userManager.UpdateAsync(user);
        // 🍪 Cookie olarak token yaz

        CreateCookie("accessToken", newAccessToken, DateTime.UtcNow.AddMinutes(60));
        CreateCookie("refreshToken", newRefreshToken, DateTime.UtcNow.AddDays(7)); // DB ile eşit
      
        return Ok(new { message = "Token yenilendi" });
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1);
                await _userManager.UpdateAsync(user);
            }
        }

        // Cookie'leri her halükarda temizle
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1) // Geçmiş bir tarih
        };

        Response.Cookies.Delete("accessToken", cookieOptions);
        Response.Cookies.Delete("refreshToken", cookieOptions);

        return Ok(new { message = "Çıkış yapıldı" });
       
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult IsAuthenticated()
    {
        return Ok(new Result<bool>
        {
            Data = User.Identity?.IsAuthenticated ?? false,
            Success = true
        });
    }
    [Authorize]
    [HttpGet]
    public IActionResult Me()
    {
        var username = User.Identity?.Name;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value);

        return Ok(new Result<object>
        {
            Data = new
            {
                userId,
                username,
                roles
            },
            Success = true
        });
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
