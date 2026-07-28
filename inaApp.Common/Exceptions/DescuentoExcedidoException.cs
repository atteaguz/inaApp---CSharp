using System;

namespace inaApp.Common.Exceptions
{
    public class DescuentoExcedidoException : Exception
    {
        public DescuentoExcedidoException() : base() { }
        public DescuentoExcedidoException(string message) : base(message) { }
        public DescuentoExcedidoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}