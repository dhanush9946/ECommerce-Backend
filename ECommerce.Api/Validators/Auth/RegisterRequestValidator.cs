using ECommerce.Application.DTOs.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ECommerce.Api.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters");

            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(email => email.Trim() == email)
                .WithMessage("Email must not contain leading or trailing spaces")
                .EmailAddress().WithMessage("Invalid email format");

            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Must(p => p.Trim() == p)
                .WithMessage("Password must not contain leading or trailing spaces")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

           
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

            
            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .Must(BeValidPhoneNumber)
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Invalid phone number");
        }

        private bool BeValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // optional field → valid

            return Regex.IsMatch(phone, @"^\d{10,15}$");
        }


    }

}
