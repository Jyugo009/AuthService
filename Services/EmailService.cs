using AuthService.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AuthService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(
                _config["SendGrid:FromEmail"],
                _config["SendGrid:FromName"]
                );
            var to = new EmailAddress(email);

            var subject = "Подтверждение регистрации";
            var plainTextContent = $@"
            Здравствуйте!

            Вы зарегистрировали аккаунт в SelfMadeService.
            Для завершения процесса регистрации перейдите по ссылке: {confirmationLink}

            Если вы не регистрировались, то проигнорируйте это письмо.

            С уважением,
            Команда SelfMadeService";

            var htmlContent = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #2c3e50;'>Подтверждение регистрации</h2>
    <p>Здравствуйте!</p>
    <p>Вы зарегистрировали аккаунт в <strong>SelfMadeService</strong>.</p>
    <p style='margin: 20px 0;'>
        <a href='{confirmationLink}' 
           style='background: #3498db; color: white; 
                  padding: 10px 20px; text-decoration: none; 
                  border-radius: 5px;'>
            Подтвердить регистрацию
        </a>
    </p>
    <p><small>Если кнопка не работает, скопируйте ссылку в браузер:<br>
       <code style='word-break: break-all;'>{confirmationLink}</code></small></p>
    <p style='color: #7f8c8d; font-size: 0.9em;'>
        Если вы не регистрировались, проигнорируйте это письмо.
    </p>
    <hr style='border: none; border-top: 1px solid #eee;'>
    <p>С уважением,<br>Команда SelfMadeService</p>
</div>";

            var msg = MailHelper.CreateSingleEmail(
                from, to, subject, plainTextContent, htmlContent
                );
            await client.SendEmailAsync(msg);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(
                _config["SendGrid:FromEmail"],
                _config["SendGrid:FromName"]
                );
            var to = new EmailAddress(email);

            var subject = "Сброс пароля";
            var plainTextContent = $@"
            Здравствуйте!

            Вы запросили сброс пароля для аккаунта в SelfMadeService.
            Для завершения процесса перейдите по ссылке: {resetLink}

            Если вы не запрашивали сброс, проигнорируйте это письмо.

            С уважением,
            Команда SelfMadeService";

            var htmlContent = $@"
<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <h2 style='color: #2c3e50;'>Сброс пароля</h2>
    <p>Здравствуйте!</p>
    <p>Вы запросили сброс пароля для аккаунта в <strong>SelfMadeService</strong>.</p>
    <p style='margin: 20px 0;'>
        <a href='{resetLink}' 
           style='background: #3498db; color: white; 
                  padding: 10px 20px; text-decoration: none; 
                  border-radius: 5px;'>
            Сбросить пароль
        </a>
    </p>
    <p><small>Если кнопка не работает, скопируйте ссылку в браузер:<br>
       <code style='word-break: break-all;'>{resetLink}</code></small></p>
    <p style='color: #7f8c8d; font-size: 0.9em;'>
        Если вы не запрашивали сброс, проигнорируйте это письмо.
    </p>
    <hr style='border: none; border-top: 1px solid #eee;'>
    <p>С уважением,<br>Команда SelfMadeService</p>
</div>";

            var msg = MailHelper.CreateSingleEmail(
                from, to, subject, plainTextContent, htmlContent
                );

            await client.SendEmailAsync(msg);
        }
    }
}
