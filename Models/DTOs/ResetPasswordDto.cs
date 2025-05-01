using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Пожалуйста, введите email-адрес.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token {  get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым.")] 
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
