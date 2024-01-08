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
        }
    }
}
