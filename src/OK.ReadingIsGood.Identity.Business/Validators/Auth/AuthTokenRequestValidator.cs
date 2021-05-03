using FluentValidation;
using OK.ReadingIsGood.Identity.Contracts.Requests;

namespace OK.ReadingIsGood.Identity.Business.Validators.Auth
{
    public class AuthTokenRequestValidator : AbstractValidator<AuthTokenRequest>
    {
        public AuthTokenRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}