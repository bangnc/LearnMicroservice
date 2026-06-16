using AuthService.Application.Commands.Auth.Login;
using AuthService.Application.DTOs.Auth;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;

        public LoginHandler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
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

            var token = _jwtService.CreateToken(user);

            return new LoginResponse
            {
                Email = user.Email,
                Token = token
            };
        }
    }
}
