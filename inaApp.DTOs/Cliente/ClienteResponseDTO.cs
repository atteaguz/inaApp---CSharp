using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.DTOs.Cliente
{
    public class ClienteResponseDTO
    {
        public int IdCliente { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
    }
}
