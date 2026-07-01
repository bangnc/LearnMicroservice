using AuthService.Application.Commands.Auth.Login;
using AuthService.Application.Common.Events;
using AuthService.Application.Common.Messaging;
using AuthService.Application.Common.Security;
using AuthService.Application.DTOs.Auth;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILoginSessionRepository _loginSessionRepository;
       // private readonly IEmailService _emailService;
        //private readonly IKafkaProducer _kafkaProducer;
        private readonly IEventBus _eventBus;
        public LoginHandler(
             UserManager<AppUser> userManager,
             SignInManager<AppUser> signInManager,
             IJwtService jwtService,
             ILoginSessionRepository loginSessionRepository,
             IUnitOfWork unitOfWork,
             IEventBus eventBus
             )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _loginSessionRepository = loginSessionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, request.Password, false);

            if (!result.Succeeded)
                throw new Exception("Invalid password");
            // Sinh OTP
            var otp = OtpGenerator.Generate();
            var session = new LoginSession
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                SessionId = Guid.NewGuid().ToString(),
                Otp = OtpGenerator.Hash(otp),
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false,
                AttemptCount = 0
            };
           

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _loginSessionRepository.AddAsync(session, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            var token = _jwtService.CreateTempToken(user, session.SessionId);
            await _eventBus.PublishAsync(
            KafkaTopics.SendOtp,
            new SendOtpEmailIntegrationEvent
            {
                Email = user.Email!,
                FullName = user.FullName ?? "",
                Otp = otp
            },
            cancellationToken);


            //await _emailService.SendOtpAsync(
            //                user.Email!,
            //                otp,
            //                cancellationToken);
            return new LoginResponse
            {
                RequiresOtp = true,
               TokenTemp = token
            };
        }
    }
}
