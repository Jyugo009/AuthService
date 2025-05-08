using AuthService.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Abstractions
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto dto);
        Task<IdentityUser?> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(string email, string token);

    }
}
