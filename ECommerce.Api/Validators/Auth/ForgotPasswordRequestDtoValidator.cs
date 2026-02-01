using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Auth;
using FluentValidation;

namespace ECommerce.Api.Validators
{
    public class ForgotPasswordRequestDtoValidator
        : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(email => email.Trim() == email)
                .WithMessage("Email must not contain leading or trailing spaces");
        }
    }
}

