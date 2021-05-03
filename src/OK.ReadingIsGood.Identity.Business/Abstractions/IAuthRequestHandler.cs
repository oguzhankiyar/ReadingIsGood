using MediatR;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;

namespace OK.ReadingIsGood.Identity.Business.Abstractions
{
    public interface IAuthRequestHandler :
        IRequestHandler<AuthTokenRequest, AuthTokenResponse>
    {

    }
}