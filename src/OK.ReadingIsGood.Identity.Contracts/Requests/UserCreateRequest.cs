using MediatR;
using OK.ReadingIsGood.Identity.Contracts.Responses;

namespace OK.ReadingIsGood.Identity.Contracts.Requests
{
    public class UserCreateRequest : IRequest<UserCreateResponse>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}