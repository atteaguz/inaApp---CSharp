using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaCreateViewModel
    {
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int ClienteId { get; set; }
        public List<SelectListItem> ClientesList { get; set; } = new List<SelectListItem>();
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Debe seleccionar un producto")]
        public int ProductoId { get; set; }
        public List<SelectListItem> ProductosList { get; set; } = new List<SelectListItem>();
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; } = 1;
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new List<FacturaDetalleViewModel>();
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }
        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }
        [Display(Name = "Impuesto Total")]
        public decimal ImpuestoTotal { get; set; }
        [Display(Name = "Total")]
        public decimal Total { get; set; }
        public bool TieneDetalles => Detalles != null && Detalles.Any();
        public string Error { get; set; }
    }
}