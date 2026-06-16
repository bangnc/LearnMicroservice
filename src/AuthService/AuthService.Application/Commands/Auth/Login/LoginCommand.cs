using AuthService.Application.DTOs.Auth;
using MediatR;

namespace AuthService.Application.Commands.Auth.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
