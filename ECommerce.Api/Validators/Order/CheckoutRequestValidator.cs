using ECommerce.Application.DTOs.Order;
using FluentValidation;

namespace ECommerce.Api.Validators.Order
{
    public class CheckoutRequestValidator : AbstractValidator<CheckoutRequestDto>
    {
        private static readonly string[] AllowedMethods = { "cod", "razorpay" };

        public CheckoutRequestValidator()
        {
            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Shipping address is required.")
                .MinimumLength(10).WithMessage("Please enter a full delivery address.")
                .MaximumLength(300).WithMessage("Address is too long.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.")
                .Must(m => AllowedMethods.Contains(m?.ToLower()))
                .WithMessage("Payment method must be 'cod' or 'razorpay'.");
        }
    }
}
