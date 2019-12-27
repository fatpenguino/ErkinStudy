using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ErkinStudy.Infrastructure.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ErkinStudy", "info@erkinstudy.kz"));
            emailMessage.To.Add(new MailboxAddress("", _configuration.GetSection("EmailSettings")["To"]));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("mail.hosting.reg.ru", 25, false);
            await client.AuthenticateAsync("info@erkinstudy.kz", "Qazaq123@");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}