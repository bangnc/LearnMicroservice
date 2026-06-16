using AuthService.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Auth.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
