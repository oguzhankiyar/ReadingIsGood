using MediatR;
using OK.ReadingIsGood.Identity.Contracts.Responses;

namespace OK.ReadingIsGood.Identity.Contracts.Requests
{
    public class AuthTokenRequest : IRequest<AuthTokenResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}