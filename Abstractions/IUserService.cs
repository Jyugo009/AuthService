using AuthService.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Abstractions
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto dto);
        Task<IdentityUser?> FindUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    }
}
