using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.Entities
{
    [Table("tbCliente")]
    public class Cliente
    {
        //propiedades de clientes
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public bool Estado { get; set; }
    }
}
