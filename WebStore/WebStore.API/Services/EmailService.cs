using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using WebStore.DTO;
using WebStore.API.Services.Contracts;

namespace WebStore.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(EmailDTO request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
