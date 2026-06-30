using AuthService.Application.Commands.Auth.VerifyOtp;
using AuthService.Application.Common.Security;
using AuthService.Application.DTOs.Auth;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Handlers.Auth
{
    public class VerifyOtpHandler
     : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoginSessionRepository _loginSessionRepository;
        private readonly IJwtService _jwtService;

        public VerifyOtpHandler(
            IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
            IJwtService jwtService,
            ILoginSessionRepository loginSessionRepository
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtService = jwtService;
            _loginSessionRepository = loginSessionRepository;
        }

        public async Task<VerifyOtpResponse> Handle(
            VerifyOtpCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Đọc TempToken
            
            var principal = _jwtService.ValidateToken(request.TempToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var sessionId = Guid.Parse(
                principal.FindFirst("sid")!.Value);

            // 2. Lấy LoginSession
            var loginSession = await _loginSessionRepository.GetBySessionIdAsync(sessionId.ToString(), cancellationToken);

            if (loginSession == null)
                throw new Exception("Session không tồn tại.");

            // 3. Kiểm tra đã xác thực chưa
            if (loginSession.IsVerified)
                throw new Exception("Session đã được xác thực.");

            // 4. Kiểm tra hết hạn
            if (loginSession.ExpiredAt < DateTime.UtcNow)
                throw new Exception("OTP đã hết hạn.");

            // 5. Kiểm tra OTP
            var hasOTP = OtpGenerator.Hash(request.Otp);
            if (loginSession.Otp != hasOTP)
                throw new Exception("OTP không đúng.");

            // 6. Đánh dấu đã xác thực
            loginSession.IsVerified = true;

            // 7. Lấy User
            var user = await _userManager.Users.FirstAsync(x => x.Id == userId, cancellationToken);

            // 8. Sinh Token
            var accessToken = _jwtService.CreateToken(user);

            var refreshToken = _jwtService.CreateRefreshToken(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new VerifyOtpResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
