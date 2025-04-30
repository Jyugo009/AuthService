using Microsoft.AspNetCore.Identity;

namespace AuthService.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(IdentityUser user);
    }
}
