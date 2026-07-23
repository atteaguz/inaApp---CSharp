using System;
using System.Collections.Generic;

namespace inaApp.DTOs.Factura
{
    public class FacturaResponseDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteCorreo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FacturaDetalleResponseDTO> Detalles { get; set; } = new List<FacturaDetalleResponseDTO>();
    }
    public class FacturaListDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public int CantidadProductos { get; set; }
    }
}