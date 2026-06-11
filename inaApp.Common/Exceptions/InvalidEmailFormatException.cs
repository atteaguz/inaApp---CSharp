using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException()
        {
        }

        public InvalidEmailFormatException(string? message) : base(message)
        {
        }

        public InvalidEmailFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
