using AuthService.Application.Commands.Auth.Register;
using AuthService.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Handlers.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        public Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new RegisterResponse
            {
                Email = request.Email,
                Message = "Register success"
            };

            return Task.FromResult(response);
        }
    }
}
