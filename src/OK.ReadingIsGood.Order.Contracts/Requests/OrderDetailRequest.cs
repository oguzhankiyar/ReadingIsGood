using MediatR;
using OK.ReadingIsGood.Order.Contracts.Responses;

namespace OK.ReadingIsGood.Order.Contracts.Requests
{
    public class OrderDetailRequest : IRequest<OrderDetailResponse>
    {
        public int Id { get; set; }
    }
}