using AuthService.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Auth.VerifyOtp
{
    public class VerifyOtpCommand : IRequest<VerifyOtpResponse>
    {
        public string TempToken { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
