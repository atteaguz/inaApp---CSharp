using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class NotaCreditoDetalleCreateViewModel
    {
        public int FacturaDetalleOriginalId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int CantidadOriginal { get; set; }

        [Display(Name = "Cantidad a Acreditar")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int CantidadAcreditar { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal MontoImpuesto { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal TotalLinea { get; set; }
        public bool Seleccionado { get; set; } = true;
    }
}