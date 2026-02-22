using ECommerce.API.DTOs;
using FluentValidation;

namespace ECommerce.Api.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        private static readonly string[] AllowedGenders = { "Men", "Women", "Unisex", "Kids" };

        public UpdateProductValidator()
        {
            // All fields are optional for updates, but if provided they must be valid

            When(x => x.Name != null, () =>
                RuleFor(x => x.Name)
                    .MaximumLength(100).WithMessage("Name must not exceed 100 characters."));

            When(x => x.Brand != null, () =>
                RuleFor(x => x.Brand)
                    .MaximumLength(50).WithMessage("Brand must not exceed 50 characters."));

            When(x => x.Category != null, () =>
                RuleFor(x => x.Category)
                    .MaximumLength(50).WithMessage("Category must not exceed 50 characters."));

            When(x => x.Price.HasValue, () =>
                RuleFor(x => x.Price!.Value)
                    .GreaterThan(0).WithMessage("Price must be greater than zero."));

            When(x => x.Stock.HasValue, () =>
                RuleFor(x => x.Stock!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative."));

            When(x => x.MaxOrderQuantity.HasValue, () =>
                RuleFor(x => x.MaxOrderQuantity!.Value)
                    .GreaterThan(0).WithMessage("Max order quantity must be at least 1.")
                    .LessThanOrEqualTo(1000).WithMessage("Max order quantity cannot exceed 1000."));

            When(x => x.Gender != null, () =>
                RuleFor(x => x.Gender)
                    .Must(g => AllowedGenders.Contains(g))
                    .WithMessage($"Gender must be one of: {string.Join(", ", AllowedGenders)}."));

            When(x => x.Description != null, () =>
                RuleFor(x => x.Description)
                    .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters."));
        }
    }
}
