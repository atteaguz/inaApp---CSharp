using System;

namespace inaApp.Common.Exceptions
{
    public class ImpuestoInvalidoException : Exception
    {
        public ImpuestoInvalidoException() : base() { }
        public ImpuestoInvalidoException(string message) : base(message) { }
        public ImpuestoInvalidoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}