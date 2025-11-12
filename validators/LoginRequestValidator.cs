using EShop.Dto;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.Data;
using Serilog;
namespace EShop.validators

{
    public class LoginRequestValidator : AbstractValidator<Dto.LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
