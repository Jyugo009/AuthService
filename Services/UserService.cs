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
        private readonly IEmailService _emailService;
        private readonly AuthDbContext _dbContext;
        public UserService(UserManager<IdentityUser> userManager, AuthDbContext dbContext, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto dto)
        {
            var existingUser = await GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError{Code = "DuplicateEmail"});

            var user = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded && !string.IsNullOrEmpty(dto.CallbackUrl))
            {
                var token = await GenerateEmailConfirmationTokenAsync(user.Email);
                var confirmationToken = $"{dto.CallbackUrl}?token={token}&email={user.Email}";
                await _emailService.SendEmailConfirmationAsync(user.Email, confirmationToken);
            }
            return result;
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
            var user = await GetUserByEmailAsync(email);
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
            var user = await GetUserByEmailAsync(email);
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await GetUserByEmailAsync (email);
            if (user == null)
                return null;

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
        {
            var user = await GetUserByEmailAsync(email);
            if(user == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound" });

            return await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}
