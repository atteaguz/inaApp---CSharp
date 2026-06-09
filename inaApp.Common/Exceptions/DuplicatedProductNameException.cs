using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class DuplicatedProductNameException : Exception
    {
        public DuplicatedProductNameException()
        {
        }

        public DuplicatedProductNameException(string? message) : base(message)
        {
        }
    }
}
