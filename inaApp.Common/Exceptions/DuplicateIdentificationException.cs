using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class DuplicateIdentificationException : Exception
    {
        public DuplicateIdentificationException()
        {
        }

        public DuplicateIdentificationException(string? message) : base(message)
        {
        }

        public DuplicateIdentificationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
