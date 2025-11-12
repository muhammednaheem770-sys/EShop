using EShop.Dto.Auth;
using FluentValidation;
using FluentValidation.Results;
using Serilog;
using EShop.Dto;

namespace EShop.validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[!@#$%^&*(),.?\"":{}|<>]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Phone number must be between 10 and 15 digits and may start with +.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        }

        protected override bool PreValidate(ValidationContext<RegisterRequestDto> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                Log.Warning("Registration validation failed: request body was null.");
                result.Errors.Add(new ValidationFailure("", "Request body cannot be null."));
                return false;
            }

            return base.PreValidate(context, result);
        }

        public override ValidationResult Validate(ValidationContext<RegisterRequestDto> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Log.Warning("Registration validation failed for {Email}: {ErrorMessage}",
                        context.InstanceToValidate?.Email,
                        error.ErrorMessage);
                }
            }
            else
            {
                Log.Information("Registration validation succeeded for {Email}", context.InstanceToValidate.Email);
            }

            return validationResult;
        }
    }
}
