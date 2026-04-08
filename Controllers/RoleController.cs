using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.Role;
using OskApi.Entities.User;
using OskApi.Shared.Result;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var mappedRoles = roles.Select(r => new ListRoleDto
            {
                Id = r.Id,
                Name = r.Name ?? ""
            }).ToList();

            return Ok(Result<List<ListRoleDto>>.Ok(mappedRoles));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateRoleDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest(Result.Fail("Rol adı boş olamaz."));

            var roleExists = await _roleManager.RoleExistsAsync(model.Name);
            if (roleExists)
                return BadRequest(Result.Fail("Bu rol zaten mevcut."));

            var result = await _roleManager.CreateAsync(new AppRole { Name = model.Name });

            if (result.Succeeded)
                return Ok(Result.Ok("Rol başarıyla eklendi."));

            return BadRequest(Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleDto model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
                return NotFound(Result.Fail("Rol bulunamadı."));

            if (role.Name != model.Name)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (roleExists)
                    return BadRequest(Result.Fail("Bu isimde bir rol zaten var."));

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    return BadRequest(Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));
            }

            return Ok(Result.Ok("Rol başarıyla güncellendi."));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound(Result.Fail("Rol bulunamadı."));

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return Ok(Result.Ok("Rol silindi."));

            return BadRequest(Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));
        }
    }
}
