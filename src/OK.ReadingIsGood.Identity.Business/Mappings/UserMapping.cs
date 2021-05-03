using System.Collections.Generic;
using AutoMapper;
using OK.ReadingIsGood.Identity.Contracts.Models;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.User;

namespace OK.ReadingIsGood.Identity.Business.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserEntity, UserCreatedEvent.UserModel>();
            CreateMap<UserEntity, UserCreatedEvent>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<UserModel, UserEntity>();

            CreateMap<UserCreateRequest, UserEntity>();

            CreateMap<List<UserEntity>, UserListResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<UserEntity, UserCreateResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}