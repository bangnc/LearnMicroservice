using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Persistence.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AuthService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendOtpAsync(
            string email,
            string otp,
            CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.DisplayName,
                    _settings.UserName));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Your OTP Code";

            message.Body = new TextPart("html")
            {
                Text = $@"
                        <h2>Email Verification</h2>

                        <p>Your OTP code is:</p>

                        <h1>{otp}</h1>

                        <p>This code will expire in 5 minutes.</p>

                        <p>If you didn't request this, please ignore this email.</p>"
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls,
                cancellationToken);

            await client.AuthenticateAsync(
                _settings.UserName,
                _settings.Password,
                cancellationToken);

            await client.SendAsync(
                message,
                cancellationToken);

            await client.DisconnectAsync(
                true,
                cancellationToken);
        }
    }
}
