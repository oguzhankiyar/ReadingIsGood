using FluentValidation;
using OK.ReadingIsGood.Identity.Contracts.Requests;

namespace OK.ReadingIsGood.Identity.Business.Validators.User
{
    public class UserListRequestValidator : AbstractValidator<UserListRequest>
    {
        public UserListRequestValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
        }
    }
}