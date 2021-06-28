namespace Application.Common.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Result
    {
        private Result(bool succeeded, string error, ErrorType errorType = ErrorType.General)
        {
            Succeeded = succeeded;
            Error = error;
            ErrorType = errorType;
        }

        public bool Succeeded { get; }

        public ErrorType ErrorType { get; }

        public string Error { get; }

        public static Result Success()
        {
            return new(true, string.Empty);
        }

        public static Result Failure(string error, ErrorType errorType = ErrorType.General)
        {
            return new(false, error, errorType);
        }
    }
}