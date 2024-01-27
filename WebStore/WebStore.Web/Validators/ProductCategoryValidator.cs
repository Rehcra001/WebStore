using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryDTO>
    {
        private CustomValidationRules _vr = new CustomValidationRules();
        public ProductCategoryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("{PropertyName} may not be empty")
                .MinimumLength(3).WithMessage("{PropertyName} must have atleast {MinLength} characters")
                .MaximumLength(100).WithMessage("{PropertyName} must have less than {MaxLength} characters")
                .Must(_vr.BeAValidName).WithMessage("{PropertyName} may only contain letters, space or dash characters.");
        }
    }
}
