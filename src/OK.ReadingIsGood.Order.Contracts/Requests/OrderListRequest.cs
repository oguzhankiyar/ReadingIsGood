using MediatR;
using OK.ReadingIsGood.Order.Contracts.Responses;
using OK.ReadingIsGood.Shared.Core.Requests;

namespace OK.ReadingIsGood.Order.Contracts.Requests
{
    public class OrderListRequest : BaseListRequest, IRequest<OrderListResponse>
    {
        public int? UserId { get; set; }
    }
}