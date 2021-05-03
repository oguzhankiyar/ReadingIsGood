using System;

namespace OK.ReadingIsGood.Shared.Core.Exceptions
{
    [Serializable]
    public class RequestNotValidatedException : Exception
    {
        public string[] Errors { get; set; } = Array.Empty<string>();

        public RequestNotValidatedException()
        {

        }

        public RequestNotValidatedException(string message) : base(message)
        {

        }

        public RequestNotValidatedException(string message, string[] errors) : base(message)
        {
            Errors = errors;
        }

        public RequestNotValidatedException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public RequestNotValidatedException(string message, string[] errors, Exception innerException) : base(message, innerException)
        {
            Errors = errors;

        }
    }
}