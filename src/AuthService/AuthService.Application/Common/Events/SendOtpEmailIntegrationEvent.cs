using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Events
{
    public sealed class SendOtpEmailIntegrationEvent
    {
        public string Email { get; init; } = "";

        public string FullName { get; init; } = "";

        public string Otp { get; init; } = "";
    }
}
