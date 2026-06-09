using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class DuplicatedNameException : Exception
    {
        public DuplicatedNameException()
        {
        }

        public DuplicatedNameException(string? message) : base(message)
        {
        }
    }
}
