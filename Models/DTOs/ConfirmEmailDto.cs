using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class ConfirmEmailDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

    }
}
