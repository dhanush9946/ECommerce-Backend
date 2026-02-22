using ECommerce.Application.DTOs.Payment;
using FluentValidation;

namespace ECommerce.Api.Validators.Payment
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequestDto>
    {
        private static readonly string[] AllowedMethods = { "cod", "Razorpay" };
        private static readonly string[] AllowedStatuses = { "success", "failed" };

        public PaymentRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Method)
                .NotEmpty().WithMessage("Payment method is required.")
                .Must(m => AllowedMethods.Contains(m))
                .WithMessage("Method must be 'cod' or 'Razorpay'.");

            RuleFor(x => x.PaymentStatus)
                .NotEmpty().WithMessage("Payment status is required.")
                .Must(s => AllowedStatuses.Contains(s?.ToLower()))
                .WithMessage("PaymentStatus must be 'success' or 'failed'.");

            // RazorpayDetails required only on Razorpay success
            When(x => x.Method == "Razorpay" && x.PaymentStatus?.ToLower() == "success", () =>
            {
                RuleFor(x => x.RazorpayDetails)
                    .NotNull().WithMessage("Razorpay details are required for successful payments.");

                RuleFor(x => x.RazorpayDetails!.RazorpayOrderId)
                    .NotEmpty().WithMessage("Razorpay order ID is required.");

                RuleFor(x => x.RazorpayDetails!.RazorpayPaymentId)
                    .NotEmpty().WithMessage("Razorpay payment ID is required.");

                RuleFor(x => x.RazorpayDetails!.RazorpaySignature)
                    .NotEmpty().WithMessage("Razorpay signature is required.");
            });
        }
    }
}
