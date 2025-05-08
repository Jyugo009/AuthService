namespace AuthService.Abstractions
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetLink);
        Task SendEmailConfirmationAsync(string email, string confirmationLink);
    }
}
