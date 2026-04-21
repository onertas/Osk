using OskApi.Entities.User;
using System.Security.Claims;

namespace OskApi.Services.Jwt
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        string CreateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
