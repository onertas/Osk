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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> n8n()
        {
            var req = new SoruRequest();
            req.Soru = "Tabela";
            req.PdfUrl = "https://www.mevzuat.gov.tr/MevzuatMetin/yonetmelik/7.5.42353.pdf";

            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://n8n.onertas.com/webhook/mevzuat-soru", req);

            // İstek başarılıysa içeriği (body) okuyup döndürüyoruz
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync(); // JSON'ı string olarak okur
                return Ok(result);
            }

            return BadRequest($"n8n isteği başarısız oldu. Durum Kodu: {response.StatusCode}");
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult test()
        {
            return Ok("test endpoint'i çalışıyor!");
        }



        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}
public class SoruRequest
{
    public string Soru { get; set; }
    public string PdfUrl { get; set; }
}