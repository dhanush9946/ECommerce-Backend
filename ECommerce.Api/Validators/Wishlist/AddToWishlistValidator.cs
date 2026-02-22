using ECommerce.Application.DTOs.Wishlist;
using FluentValidation;

namespace ECommerce.Api.Validators.Wishlist
{
    public class AddToWishlistValidator : AbstractValidator<AddToWishlistDto>
    {
        public AddToWishlistValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid product.");
        }
    }
}
