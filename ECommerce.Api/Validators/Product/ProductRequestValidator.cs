using ECommerce.Application.DTOs.Product;
using FluentValidation;

namespace ECommerce.Api.Validators.Product
{
    public class ProductRequestValidator : AbstractValidator<ProductRequestDto>
    {
        private static readonly string[] AllowedGenders = { "Men", "Women", "Unisex", "Kids" };
        private static readonly string[] AllowedStatuses = { "active", "inactive" };

        public ProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(50).WithMessage("Brand must not exceed 50 characters.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(x => x.MaxOrderQuantity)
                .GreaterThan(0).WithMessage("Max order quantity must be at least 1.")
                .LessThanOrEqualTo(1000).WithMessage("Max order quantity cannot exceed 1000.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => AllowedGenders.Contains(g))
                .WithMessage($"Gender must be one of: {string.Join(", ", AllowedGenders)}.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => AllowedStatuses.Contains(s?.ToLower()))
                .WithMessage("Status must be 'active' or 'inactive'.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Image URL is required.");
        }
    }
}
