using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskApi.Dtos.User;
using OskApi.Entities.User;
using OskApi.Shared.Result;

namespace OskApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var mappedUsers = new List<SystemListUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                mappedUsers.Add(new SystemListUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName ?? "",
                    Email = user.Email ?? "",
                    UserName = user.UserName ?? "",
                    Roles = roles.ToList()
                });
            }

            return Ok(Result<List<SystemListUserDto>>.Ok(mappedUsers));
        }

        [HttpPost]
        public async Task<IActionResult> Add(SystemCreateUserDto model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
                return BadRequest(Result.Fail("Bu kullanıcı adı zaten alınmış."));

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
                return BadRequest(Result.Fail(string.Join(", ", createResult.Errors.Select(e => e.Description))));

            if (model.Roles != null && model.Roles.Any())
            {
                foreach (var roleName in model.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(roleName))
                        await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return Ok(Result.Ok("Kullanıcı başarıyla eklendi."));
        }

        [HttpPost]
        public async Task<IActionResult> Update(SystemUpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound(Result.Fail("Kullanıcı bulunamadı."));

            var userByName = await _userManager.FindByNameAsync(model.UserName);
            if(userByName != null && userByName.Id != user.Id)
            {
                return BadRequest(Result.Fail("Bu kullanıcı adı başka bir hesaba ait."));
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(Result.Fail(string.Join(", ", updateResult.Errors.Select(e => e.Description))));

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!passResult.Succeeded)
                    return BadRequest(Result.Fail("Şifre güncellenemedi: " + string.Join(", ", passResult.Errors.Select(e => e.Description))));
            }

            // Update Roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = model.Roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(model.Roles).ToList();

            if (rolesToAdd.Any())
                await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToRemove.Any())
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return Ok(Result.Ok("Kullanıcı güncellendi."));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(Result.Fail("Kullanıcı bulunamadı."));

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(Result.Ok("Kullanıcı silindi."));

            return BadRequest(Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));
        }
    }
}
