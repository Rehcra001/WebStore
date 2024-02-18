using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class CompanyEFTDetailValidator : AbstractValidator<CompanyEFTDTO>
    {
        public CompanyEFTDetailValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Bank)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.AccountType)
                .NotEmpty().WithMessage("{PropertyName} may not be empty")
                .Length(2, 50).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters."); ;

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 25).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.BranchCode)
                .NotEmpty().WithMessage("{PropertyName} may not be empty.")
                .Length(2, 15).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters."); ;
        }
    }
}
