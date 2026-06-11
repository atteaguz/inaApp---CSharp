using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.DTOs.Producto
{
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
    }
}
