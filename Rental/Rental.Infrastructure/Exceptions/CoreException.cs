using System;

namespace Rental.Infrastructure.Exceptions
{
    public class CoreException : Exception
    {
        public string Code { get; }

        protected CoreException()
        {
        }

        public CoreException(string code, string message) : base(message)
        {
            Code = code;
        }

        public CoreException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
