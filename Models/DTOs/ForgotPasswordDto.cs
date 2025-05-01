using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Пожалуйста, введите email-адрес.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
