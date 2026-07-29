using System.ComponentModel.DataAnnotations;
using inaApp.Common.Enums;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class NotaCreditoCreateViewModel
    {
        public int FacturaOriginalId { get; set; }
        public string FacturaOriginalNumero { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }

        [Display(Name = "Motivo de la Nota de Crédito")]
        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(1000, ErrorMessage = "El motivo no puede exceder los 1000 caracteres")]
        public string Motivo { get; set; }

        public List<NotaCreditoDetalleCreateViewModel> Detalles { get; set; } = new List<NotaCreditoDetalleCreateViewModel>();

        // Totales
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }

        public bool TieneDetalles => Detalles != null && Detalles.Any();
        public string Error { get; set; }
    }
}