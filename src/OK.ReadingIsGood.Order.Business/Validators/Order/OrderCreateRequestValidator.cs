using System.Linq;
using FluentValidation;
using OK.ReadingIsGood.Order.Contracts.Requests;

namespace OK.ReadingIsGood.Order.Business.Validators.Order
{
    public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
    {
        public OrderCreateRequestValidator()
        {
            RuleFor(x => x.Items)
                .Must(x => x.Any())
                .WithMessage("The items must be specified.");

            RuleFor(x => x.Items)
                .Must(x => x.All(y => y.ProductId > 0 && y.Quantity > 0))
                .WithMessage("The items must be valid.");
        }
    }
}