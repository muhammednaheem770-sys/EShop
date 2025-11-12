using FluentValidation;
using System.ComponentModel.DataAnnotations;
using EShop.Dto;

namespace EShop.validators
{
    public class CreateProductDtoRequestValidator : AbstractValidator<CreateProductDtoRequest>
    {
        public CreateProductDtoRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        }
    }
}
