using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs.Factura
{
    public class NotaCreditoDetalleCreateDTO
    {
        [Required(ErrorMessage = "El ID del detalle original es obligatorio")]
        public int FacturaDetalleOriginalId { get; set; }

        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        // Campos que se llenan en el Service (desde la factura original)
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal MontoImpuesto { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal TotalLinea { get; set; }
        public string ProductoNombre { get; set; }
        public int CantidadOriginal { get; set; } // Para validación
    }
}