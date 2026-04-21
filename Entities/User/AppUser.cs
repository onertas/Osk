using Microsoft.AspNetCore.Identity;

namespace OskApi.Entities.User
{
    public class AppUser:IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
