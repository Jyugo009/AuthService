using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Введите ваш Email-адрес")]
        [EmailAddress(ErrorMessage = "Введен некорректный адрес, повторите попытку")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Введите ваш пароль")]
        public string Password { get; set; }
    }
}
