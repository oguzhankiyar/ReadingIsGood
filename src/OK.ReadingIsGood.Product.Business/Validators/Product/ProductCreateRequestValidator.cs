using FluentValidation;
using OK.ReadingIsGood.Product.Contracts.Requests;

namespace OK.ReadingIsGood.Product.Business.Validators.Product
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.StockCount)
                .GreaterThanOrEqualTo(0);
        }
    }
}