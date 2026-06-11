using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class InvalidIdentificationException : Exception
    {
        public InvalidIdentificationException()
        {
        }

        public InvalidIdentificationException(string? message) : base(message)
        {
        }

        public InvalidIdentificationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
