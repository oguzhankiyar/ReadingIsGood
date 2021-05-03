using FluentValidation;
using OK.ReadingIsGood.Identity.Contracts.Requests;

namespace OK.ReadingIsGood.Identity.Business.Validators.User
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);
        }
    }
}