using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace inaApp.Entities
{
    [Table("tbCategoria")]
    public class Categoria
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required (ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; }
        public bool Estado { get; set; } = true;

        // relacion entre Categoria y Producto - 1 a *
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}