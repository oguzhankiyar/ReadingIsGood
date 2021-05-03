using FluentValidation;
using OK.ReadingIsGood.Product.Contracts.Requests;

namespace OK.ReadingIsGood.Product.Business.Validators.Product
{
    public class ProductListRequestValidator : AbstractValidator<ProductListRequest>
    {
        public ProductListRequestValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
        }
    }
}