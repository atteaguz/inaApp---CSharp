using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class RequiredFieldMissingException : Exception
    {
        public RequiredFieldMissingException()
        {
        }

        public RequiredFieldMissingException(string? message) : base(message)
        {
        }

        public RequiredFieldMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
