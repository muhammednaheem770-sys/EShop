using EShop.Dto;
using EShop.validators;
using FluentValidation;

namespace EShop.validators
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters long")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");
        }
    }
}
