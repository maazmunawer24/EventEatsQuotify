using EventEatsQuotify.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using EventEatsQuotify.Models;

namespace EventEatsQuotify.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrEmpty(htmlMessage))
                throw new ArgumentNullException(nameof(htmlMessage));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // Log error if email sending fails
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw new InvalidOperationException($"Error sending email: {ex.Message}", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
