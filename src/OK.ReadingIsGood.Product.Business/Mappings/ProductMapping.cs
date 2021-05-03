using System.Collections.Generic;
using AutoMapper;
using OK.ReadingIsGood.Product.Contracts.Models;
using OK.ReadingIsGood.Product.Contracts.Requests;
using OK.ReadingIsGood.Product.Contracts.Responses;
using OK.ReadingIsGood.Product.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Product;

namespace OK.ReadingIsGood.Product.Business.Mappings
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductEntity, ProductModel>();
            CreateMap<ProductEntity, ProductUpdatedEvent.ProductModel>();
            CreateMap<ProductEntity, ProductUpdatedEvent>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ProductModel, ProductEntity>();

            CreateMap<ProductCreateRequest, ProductEntity>();

            CreateMap<List<ProductEntity>, ProductListResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ProductEntity, ProductCreateResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ProductEntity, ProductEditResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}