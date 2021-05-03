using MediatR;
using OK.ReadingIsGood.Identity.Contracts.Responses;
using OK.ReadingIsGood.Shared.Core.Requests;

namespace OK.ReadingIsGood.Identity.Contracts.Requests
{
    public class UserListRequest : BaseListRequest, IRequest<UserListResponse>
    {

    }
}