using System.Collections.Generic;
using MediatR;
using OK.ReadingIsGood.Order.Contracts.Models;
using OK.ReadingIsGood.Order.Contracts.Responses;

namespace OK.ReadingIsGood.Order.Contracts.Requests
{
    public class OrderCreateRequest : IRequest<OrderCreateResponse>
    {
        public List<OrderItemModel> Items { get; set; }

        public OrderCreateRequest()
        {
            Items = new List<OrderItemModel>();
        }
    }
}