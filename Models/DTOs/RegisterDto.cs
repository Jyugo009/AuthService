using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Пожалуйста, введите email-адрес.")]
        [EmailAddress(ErrorMessage = "Email-адрес введен некорректно.")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [MinLength(8, ErrorMessage = "Пароль должен быть от 8 символов.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
