using ECommerce.Application.DTOs.Cart;
using FluentValidation;

namespace ECommerce.Api.Validators.Cart
{
    public class UpdateCartQuantityValidator : AbstractValidator<UpdateCartQuantityDto>
    {
        private static readonly string[] AllowedActions = { "increase", "decrease" };

        public UpdateCartQuantityValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid product.");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action is required.")
                .Must(a => AllowedActions.Contains(a?.ToLower()))
                .WithMessage("Action must be 'increase' or 'decrease'.");
        }
    }
}
