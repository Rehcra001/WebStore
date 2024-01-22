using FluentValidation;
using WebStore.DTO;

namespace WebStore.WEB.Validators
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} may not be empty");

            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} may not be empty");

            RuleFor(x => x.Picture).NotEmpty().WithMessage("{PropertyName} may not be empty");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("{PropertyName} may not be empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");

            RuleFor(x => x.QtyInStock)
                .NotEmpty().WithMessage("{PropertyName} may not be empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("{PropertyName} may not be empty");

            RuleFor(x => x.UnitPerId).GreaterThan(0).WithMessage("{PropertyName} may not be empty");
        }
    }
}
