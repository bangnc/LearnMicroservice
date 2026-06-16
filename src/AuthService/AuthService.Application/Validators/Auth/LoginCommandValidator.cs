using AuthService.Application.Commands.Auth.Login;
using FluentValidation;

namespace AuthService.Application.Validators.Auth
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(6).WithMessage("Password phải >= 6 ký tự");
        }
    }
}
