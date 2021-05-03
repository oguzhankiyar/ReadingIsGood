using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OK.ReadingIsGood.Shared.Core.Exceptions;

namespace OK.ReadingIsGood.Identity.Business.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> :
           IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .Select(x => x.ErrorMessage)
                .ToArray();

            if (errors.Any())
            {
                throw new RequestNotValidatedException("Not Validated", errors);
            }

            return next();
        }
    }
}