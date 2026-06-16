using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.DTOs.Categoria
{
    public class CategoriaResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // relacion entre Categoria y Producto - 1 a *
        //public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
