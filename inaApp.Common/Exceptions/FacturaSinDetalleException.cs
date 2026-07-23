using System;

namespace inaApp.Common.Exceptions
{
    public class FacturaSinDetalleException : Exception
    {
        public FacturaSinDetalleException() : base() { }
        public FacturaSinDetalleException(string message) : base(message) { }
        public FacturaSinDetalleException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}