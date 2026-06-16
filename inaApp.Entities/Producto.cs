using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.Entities
{
    [Table("tbProducto")]
    public class Producto
    {
        //propiedades: valores que describen al producto
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        [StringLength(100, ErrorMessage ="El nombre debe tener entre 3 y 100 caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
        public bool Estado { get; set; } = true;

        //relacion entre Producto y Categoria - * a 1
        public int CategoriaId { get; set; }​
        public Categoria Categoria​ { get; set; } = null!;
    }
}
