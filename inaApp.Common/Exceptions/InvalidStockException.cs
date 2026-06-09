using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.Exceptions
{
    public class InvalidStockException : Exception
    {
        public InvalidStockException()
        {
        }

        public InvalidStockException(string? message) : base(message)
        {
        }
    }
}
