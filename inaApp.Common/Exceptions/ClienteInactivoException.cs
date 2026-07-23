using System;

namespace inaApp.Common.Exceptions
{
    public class ClienteInactivoException : Exception
    {
        public ClienteInactivoException() : base() { }
        public ClienteInactivoException(string message) : base(message) { }
        public ClienteInactivoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}