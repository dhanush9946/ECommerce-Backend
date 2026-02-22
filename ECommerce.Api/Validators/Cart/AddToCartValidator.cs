using ECommerce.Application.DTOs.Cart;
using FluentValidation;

namespace ECommerce.Api.Validators.Cart
{
    public class AddToCartValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid product.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100.");
        }
    }
}
