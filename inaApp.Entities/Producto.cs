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
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
    }
}
