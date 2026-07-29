using System;
using System.Collections.Generic;
using inaApp.Common.Enums;

namespace inaApp.DTOs.Factura
{
    public class NotaCreditoResponseDTO
    {
        public int Id { get; set; }
        public int FacturaOriginalId { get; set; }
        public string FacturaOriginalNumero { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string Motivo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }

        public TipoDocumentoEnum TipoDocumento => TipoDocumentoEnum.NotaCreditoElectronica;
        public string TipoDocumentoNombre => "Nota de Crédito Electrónica";

        public List<NotaCreditoDetalleResponseDTO> Detalles { get; set; } = new List<NotaCreditoDetalleResponseDTO>();
    }
}