using MediatR;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;

namespace OK.ReadingIsGood.Identity.Business.Abstractions
{
    public interface IUserRequestHandler :
        IRequestHandler<UserListRequest, UserListResponse>,
        IRequestHandler<UserCreateRequest, UserCreateResponse>
    {

    }
}