using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationDTO>
    {
        private CustomValidationRules _vr = new CustomValidationRules();
        public UserRegistrationValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            //Email Address
            RuleFor(u => u.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .EmailAddress().WithMessage("{PropertyName} is not valid.")
                .Must(_vr.BeAValidEmailCharacter).WithMessage("{PropertyName} contains invalid characters! ';' or '--'");

            //Confirm Email Address
            RuleFor(u => u.ConfirmEmailAddress).Equal(u => u.EmailAddress).WithMessage("{PropertyName} does not match Email Address.");

            //Password
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .MinimumLength(8).WithMessage("{PropertyName} must contain at least {MinLength} characters.");

            //Confirm Password
            RuleFor(u => u.ConfirmPassword).Equal(u => u.Password).WithMessage("{PropertyName} does not match Password.");

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
        }


    }
}
