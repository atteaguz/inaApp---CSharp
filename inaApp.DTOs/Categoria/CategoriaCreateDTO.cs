using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace inaApp.DTOs.Categoria
{
    public class CategoriaCreateDTO
    {

        [Required (ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; }

        // relacion entre Categoria y Producto - 1 a *
        //public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
