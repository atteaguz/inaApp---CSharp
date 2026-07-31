using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.DTOs.Factura
{
    public class NotaCreditoCreateDTO
    {
        [Required(ErrorMessage = "La factura original es obligatoria")]
        public int FacturaOriginalId { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(1000, ErrorMessage = "El motivo no puede exceder los 1000 caracteres")]
        public string Motivo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un producto")]
        public List<NotaCreditoDetalleCreateDTO> Detalles { get; set; } = new List<NotaCreditoDetalleCreateDTO>();

        // Campos calculados
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImpuestoTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
    }
}