using inaApp.Common.Enums;
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
        //campo agregado
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public string TipoDocumentoNombre => TipoDocumento switch
        {
            TipoDocumentoEnum.FacturaElectronica => "Factura Electrónica",
            TipoDocumentoEnum.NotaCreditoElectronica => "Nota de Crédito Electrónica",
            _ => TipoDocumento.ToString()
        };
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FacturaDetalleResponseDTO> Detalles { get; set; } = new List<FacturaDetalleResponseDTO>();
        //relacion con notas de credito
        public List<NotaCreditoResponseDTO> NotasCredito { get; set; } = new List<NotaCreditoResponseDTO>();
    }
    public class FacturaListDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public int CantidadProductos { get; set; }

        //campos agregados
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public string TipoDocumentoNombre => TipoDocumento switch
        {
            TipoDocumentoEnum.FacturaElectronica => "Factura Electrónica",
            TipoDocumentoEnum.NotaCreditoElectronica => "Nota de Crédito Electrónica",
            _ => TipoDocumento.ToString()
        };
    }
}