using MediatR;
using OK.ReadingIsGood.Order.Contracts.Requests;
using OK.ReadingIsGood.Order.Contracts.Responses;

namespace OK.ReadingIsGood.Order.Business.Abstractions
{
    public interface IOrderRequestHandler :
        IRequestHandler<OrderListRequest, OrderListResponse>,
        IRequestHandler<OrderDetailRequest, OrderDetailResponse>,
        IRequestHandler<OrderCreateRequest, OrderCreateResponse>
    {
    }
}