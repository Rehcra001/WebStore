using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class AddressValidator : AbstractValidator<AddressDTO>
    {
        public AddressValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.AddressLine1)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(5, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(a => a.AddressLine2).NotEqual(a => a.AddressLine1).WithMessage("{PropertyName} may not be the same as Address Line 1.");

            RuleFor(a => a.Suburb)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(3, 50).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(3, 50).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(a => a.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 15).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(a => a.Country)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(3, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");
        }
    }
}
