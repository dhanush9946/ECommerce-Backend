using ECommerce.Application.DTOs.Auth;
using FluentValidation;

namespace ECommerce.API.Validators.Auth;

public class LoginRequestValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty().WithMessage("Email is required")
    .Must(email => email.Trim() == email)
    .WithMessage("Email must not contain leading or trailing spaces")
    .EmailAddress().WithMessage("Invalid email format");


        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
