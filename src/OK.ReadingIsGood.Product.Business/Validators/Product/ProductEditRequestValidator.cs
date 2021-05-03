using FluentValidation;
using OK.ReadingIsGood.Product.Contracts.Requests;

namespace OK.ReadingIsGood.Product.Business.Validators.Product
{
    public class ProductEditRequestValidator : AbstractValidator<ProductEditRequest>
    {
        public ProductEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.StockCount)
                .GreaterThanOrEqualTo(0);
        }
    }
}