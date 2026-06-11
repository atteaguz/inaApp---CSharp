using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class InvalidPhoneFormatException : Exception
    {
        public InvalidPhoneFormatException()
        {
        }

        public InvalidPhoneFormatException(string? message) : base(message)
        {
        }

        public InvalidPhoneFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
