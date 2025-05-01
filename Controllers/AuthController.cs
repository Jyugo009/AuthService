using AuthService.Abstractions;
using AuthService.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtSetvice;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtSetvice = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(dto);

            if (!result.Succeeded)
            {
                var duplicateEmailError = result.Errors.FirstOrDefault(e => e.Code == "DuplicateEmail");
                if(duplicateEmailError != null)
                {
                    return Conflict(new { Message = duplicateEmailError.Description });
                }

                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Пользователь зарегистрирован!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.FindUserByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Данные не верны.");

            var isPasswordValid = await _userService.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                return Unauthorized("Данные не верны.");

            var token = _jwtSetvice.GenerateToken(user);
            return Ok(new {Token = token});
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var token = await _userService.GeneratePasswordResetTokenAsync(dto.Email);
            
            return token == null ? BadRequest("Пользователь не найден!") : Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);

            return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
        }
    }
}
