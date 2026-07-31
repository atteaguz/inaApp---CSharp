using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.DTOs.Producto
{
    public class ProductoCreateDTO
    {
        //propiedades: valores que describen al producto
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 100000000.00, ErrorMessage = "El precio del producto debe estar entre 1 y 100 millones")]
        public decimal Precio { get; set; } = 1;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        //nuevos campos agregados
        [Required(ErrorMessage = "El tipo de impuesto es obligatorio")]
        public TipoImpuestoEnum TipoImpuesto { get; set; }

        [Required(ErrorMessage = "El porcentaje de impuesto es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje de impuesto debe estar entre 0 y 100")]
        public decimal PorcentajeImpuesto { get; set; }

        [Required(ErrorMessage = "El descuento máximo es obligatorio")]
        [Range(0, 100, ErrorMessage = "El descuento máximo debe estar entre 0 y 100")]
        public decimal DescuentoMaximo { get; set; }
    }
}
