using AutoMapper;
using OK.ReadingIsGood.Order.Contracts.Models;
using OK.ReadingIsGood.Order.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Order;

namespace OK.ReadingIsGood.Order.Business.Mappings
{
    public class OrderItemMapping : Profile
    {
        public OrderItemMapping()
        {
            CreateMap<OrderItemEntity, OrderItemModel>();
            CreateMap<OrderItemEntity, OrderCreatedEvent.OrderItemModel>();
            CreateMap<OrderItemModel, OrderItemEntity>();
        }
    }
}