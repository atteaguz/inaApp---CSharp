using System;

namespace inaApp.Common.Exceptions
{
    public class DuplicadoProductoException : Exception
    {
        public DuplicadoProductoException() : base() { }
        public DuplicadoProductoException(string message) : base(message) { }
        public DuplicadoProductoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}