using System.Data;

namespace AuthService.Models
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
