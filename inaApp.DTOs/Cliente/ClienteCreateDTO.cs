using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.DTOs.Cliente
{
    public class ClienteCreateDTO
    {
        //propiedades de cliente
        [Required(ErrorMessage = "El tipo de identificacion es obligatorio")]
        public TipoIdentificacionEnum TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El numero de identificacion es obligatorio")]
        [MaxLength(20)]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(50)]
        public string PrimerApellido { get; set; }

        [MaxLength(50)]
        public string? SegundoApellido { get; set; }

        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string? CorreoElectronico { get; set; }

        [MaxLength(20)]
        [Phone(ErrorMessage = "El número de teléfono no es válido")]
        public string? Telefono { get; set; }
    }
}
