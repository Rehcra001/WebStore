using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class CustomerDetailValidator : AbstractValidator<CustomerDTO>
    {
        private CustomValidationRules _vr = new CustomValidationRules();

        public CustomerDetailValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            //First Name
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 100).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}.")
                .Must(_vr.BeAValidName).WithMessage("{PropertyName} may only contain letters, space or dash characters.");

            //Last Name
            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 100).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength}.")
                .Must(_vr.BeAValidName).WithMessage("{PropertyName} may only contain letters, space or dash characters.");

            //Phone Number
            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.");

            //Email Address
            RuleFor(u => u.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .EmailAddress().WithMessage("{PropertyName} is not valid.")
                .Must(_vr.BeAValidEmailCharacter).WithMessage("{PropertyName} contains invalid characters! ';' or '--'");

        }
    }
}
