using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.DTOs.Producto
{
    public class ProductoUpdateDTO
    {
        //propiedades: valores que describen al producto
        [Required(ErrorMessage = "El id es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }
    }
}
