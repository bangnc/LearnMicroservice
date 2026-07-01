using AuthService.Application.Common.Events;
using AuthService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Messaging.Handlers
{
    public class SendOtpHandler
    {
        private readonly IEmailService _emailService;

        public SendOtpHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(SendOtpEmailIntegrationEvent message)
        {
            await _emailService.SendOtpAsync(
                message.Email,
                message.Otp);
        }
    }
}
