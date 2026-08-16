using AuthService.Application.Commands.Auth.Register;
using AuthService.Application.Common.Events;
using AuthService.Application.DTOs.Auth;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Handlers.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterHandler(UserManager<AppUser> userManager, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Message = "User with this email already exists.",
                    Email = request.Email
                };
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = new AppUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = request.FullName,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        Message = "Register Fail",
                        Email = request.Email
                    };
                }

                var integrationEvent = new UserRegisteredIntegrationEvent
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName ?? ""
                };

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    EventType = "UserRegistered",
                    Payload = System.Text.Json.JsonSerializer.Serialize(integrationEvent),
                    CreatedAt = DateTime.UtcNow,
                    RetryCount = 0
                };

                await _outboxRepository.AddAsync(
                                                outboxMessage,
                                                cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return new RegisterResponse
                {
                    Message = "Register Success",
                    Email = request.Email
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }



        }
    }
}
