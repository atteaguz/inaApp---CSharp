using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Entities
{
    public class Cliente
    {
        //propiedades de clientes
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public bool Estado { get; set; }
    }
}
