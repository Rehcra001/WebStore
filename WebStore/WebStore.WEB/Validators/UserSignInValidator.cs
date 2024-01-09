using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class UserSignInValidator : AbstractValidator<UserSignInDTO>
    {
        private CustomValidationRules _vr = new CustomValidationRules();

        public UserSignInValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            //Email Address
            RuleFor(u => u.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .EmailAddress().WithMessage("{PropertyName} is not valid.")
                .Must(_vr.BeAValidEmailCharacter).WithMessage("{PropertyName} contains invalid characters! ';' or '--'");

            //Password
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .MinimumLength(8).WithMessage("{PropertyName} must contain at least {MinLength} characters.");
        }
    }
}
