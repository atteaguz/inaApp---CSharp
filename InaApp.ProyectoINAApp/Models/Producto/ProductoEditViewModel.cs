using inaApp.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoEditViewModel
    {
        [Required(ErrorMessage = "El Id del producto es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Nombre del Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del producto debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Precio del Producto")]
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        [Range(0.01, 1000000.00, ErrorMessage = "El precio del producto debe estar entre 0.01 y 1000000.00.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; } = 1;

        [Display(Name = "Descripción del Producto")]
        public string? Descripcion { get; set; }

        [Display(Name = "Stock del Producto")]
        [Required(ErrorMessage = "El stock del producto es obligatorio.")]
        [Range(1, 10000, ErrorMessage = "El stock del producto debe estar entre 1 y 10000.")]
        public int Stock { get; set; } = 1;

        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        //campos agregados para el impuesto y descuento
        [Display(Name = "Tipo de Impuesto")]
        [Required(ErrorMessage = "El tipo de impuesto es obligatorio")]
        public TipoImpuestoEnum TipoImpuesto { get; set; }

        [Display(Name = "Porcentaje de Impuesto")]
        [Required(ErrorMessage = "El porcentaje de impuesto es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje de impuesto debe estar entre 0 y 100")]
        public decimal PorcentajeImpuesto { get; set; }

        [Display(Name = "Descuento Máximo Permitido")]
        [Required(ErrorMessage = "El descuento máximo es obligatorio")]
        [Range(0, 100, ErrorMessage = "El descuento máximo debe estar entre 0 y 100")]
        public decimal DescuentoMaximo { get; set; }
    }
}
