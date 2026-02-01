using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Auth;
using FluentValidation;

namespace ECommerce.Api.Validators
{
    public class ResetPasswordRequestDtoValidator
        : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Reset token is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character");
        }
    }
}
