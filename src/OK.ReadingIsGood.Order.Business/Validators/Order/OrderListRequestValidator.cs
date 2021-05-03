using FluentValidation;
using OK.ReadingIsGood.Order.Contracts.Requests;

namespace OK.ReadingIsGood.Order.Business.Validators.Order
{
    public class OrderListRequestValidator : AbstractValidator<OrderListRequest>
    {
        public OrderListRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .When(x => x.UserId.HasValue);

            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
        }
    }
}