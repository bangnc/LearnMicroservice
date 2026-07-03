using AuthService.Application.Commands.Auth.Login;
using AuthService.Application.Commands.Auth.Register;
using AuthService.Application.Commands.Auth.VerifyOtp;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }
    }
    //[ApiController]
    //[Route("api/auth")]
    //public class AuthController : ControllerBase
    //{
    //    private readonly IAuthServiceManager _authService;
    //    public AuthController(IAuthServiceManager authService)
    //    {
    //        _authService = authService;
    //    }
    //    [HttpPost("register")]
    //    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    //    {
    //        var result = await _authService.RegisterAsync(request.Email, request.Password);
    //        return Ok(result);
    //    }

    //    [HttpPost("login")]
    //    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    //    {
    //        var result = await _authService.LoginAsync(request.Email, request.Password);
    //        return Ok(result);
    //    }
    //}
}
