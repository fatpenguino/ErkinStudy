﻿using System;
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

        public async Task SendEmailAsync(string subject, string message, string to = null)
        {
            using var client = new SmtpClient();
            try
            {
                var emailMessage = new MimeMessage();
                to ??= _configuration.GetSection("EmailSettings")["To"];
                emailMessage.From.Add(new MailboxAddress("ErkinStudy", "info@erkinstudy.kz"));
                emailMessage.To.Add(new MailboxAddress("", to));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };
                await client.ConnectAsync("mail.hosting.reg.ru", 587, false);
                await client.AuthenticateAsync("info@erkinstudy.kz", "Qazaq123@");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                await client.DisconnectAsync(true);
                throw e;
            }
        }
    }
}