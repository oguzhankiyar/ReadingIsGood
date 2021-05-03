using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OK.ReadingIsGood.Shared.Core.Exceptions;
using OK.ReadingIsGood.Shared.Core.Responses;

namespace OK.ReadingIsGood.Host.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RequestNotValidatedException requestNotValidatedException)
            {
                var hasAnyError = requestNotValidatedException.Errors != null && requestNotValidatedException.Errors.Any();
                var response = new BaseResponse
                {
                    Status = false,
                    Message = "Not Validated",
                    Errors = hasAnyError ? requestNotValidatedException.Errors : new string[] { requestNotValidatedException.Message }
                };
                context.Result = new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            else if (context.Exception is ResourceNotFoundException entityNotFoundException)
            {
                var response = new BaseResponse
                {
                    Status = false,
                    Message = "Not Found",
                    Errors = new string[] { entityNotFoundException.Message }
                };
                context.Result = new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
            }
            else
            {
                var response = new BaseResponse
                {
                    Status = false,
                    Message = "Unhandled Exception",
                    Errors = new string[] { context.Exception.Message }
                };
                context.Result = new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

            context.ExceptionHandled = true;
        }
    }
}