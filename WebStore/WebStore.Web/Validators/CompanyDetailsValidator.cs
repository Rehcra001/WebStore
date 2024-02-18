using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class CompanyDetailsValidator : AbstractValidator<CompanyDetailDTO>
    {
        private CustomValidationRules _vr = new CustomValidationRules();
        public CompanyDetailsValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.CompanyLogo)
                .NotEmpty().WithMessage("{PropertyName} may not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .EmailAddress().WithMessage("{PropertyName} is not valid.")
                .Must(_vr.BeAValidEmailCharacter).WithMessage("{PropertyName} contains invalid characters! ';' or '--'");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.");
        }
    }
}
