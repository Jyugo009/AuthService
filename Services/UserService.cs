using AuthService.Abstractions;
using AuthService.Models;
using AuthService.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthDbContext _dbContext;
        public UserService(UserManager<IdentityUser> userManager, AuthDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                });
            }

            var user = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }
        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            await _dbContext.PasswordResetTokens.AddAsync(resetToken);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound" });

            var resetToken = await _dbContext.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id && t.Token == token && !t.IsUsed);
            if (resetToken == null)
                return IdentityResult.Failed(new IdentityError { Code = "InvalidOrExpiredToken" });

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if(result.Succeeded)
            {
                resetToken.IsUsed = true;
                await _dbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}
