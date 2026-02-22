using ECommerce.Application.DTOs.Order;
using FluentValidation;

namespace ECommerce.Api.Validators.Order
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusDto>
    {
        private static readonly string[] AllowedStatuses =
            { "Pending", "Placed", "Shipped", "Delivered", "Cancelled" };

        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => AllowedStatuses.Contains(s))
                .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}.");
        }
    }
}
