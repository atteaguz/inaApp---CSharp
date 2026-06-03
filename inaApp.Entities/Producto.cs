using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Entities
{
    public class Producto
    {
        //propiedades: valores que describen al producto
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
    }
}
