using System.Collections.Generic;
using AutoMapper;
using OK.ReadingIsGood.Order.Contracts.Models;
using OK.ReadingIsGood.Order.Contracts.Requests;
using OK.ReadingIsGood.Order.Contracts.Responses;
using OK.ReadingIsGood.Order.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Order;

namespace OK.ReadingIsGood.Order.Business.Mappings
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderEntity, OrderModel>();
            CreateMap<OrderEntity, OrderCreatedEvent.OrderModel>();
            CreateMap<OrderEntity, OrderCreatedEvent>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<OrderModel, OrderEntity>();

            CreateMap<OrderCreateRequest, OrderEntity>();

            CreateMap<List<OrderEntity>, OrderListResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<OrderEntity, OrderDetailResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<OrderEntity, OrderCreateResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}