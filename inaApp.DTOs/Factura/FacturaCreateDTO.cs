using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs.Factura
{
    public class FacturaCreateDTO
    {
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Debe agregar al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe agregar al menos un producto")]
        public List<FacturaDetalleCreateDTO> Detalles { get; set; } = new List<FacturaDetalleCreateDTO>();

        public DateTime Fecha { get; set; } = DateTime.Now;
        //nuevo campo
        public TipoDocumentoEnum TipoDocumento { get; set; } = TipoDocumentoEnum.FacturaElectronica;

        //campos calculados, se usan en el service
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
    }
}