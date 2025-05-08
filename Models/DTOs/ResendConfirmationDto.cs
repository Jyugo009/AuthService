using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class ResendConfirmationDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CallbackUrl { get; set; }

    }
}
