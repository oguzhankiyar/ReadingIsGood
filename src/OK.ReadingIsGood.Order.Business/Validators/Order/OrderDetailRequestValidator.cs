using FluentValidation;
using OK.ReadingIsGood.Order.Contracts.Requests;

namespace OK.ReadingIsGood.Order.Business.Validators.Order
{
    public class OrderDetailRequestValidator : AbstractValidator<OrderDetailRequest>
    {
        public OrderDetailRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}