using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static inaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteCreateViewModel
    {
        [Display(Name = "Tipo de Identificación")]
        [Required(ErrorMessage = "El tipo de identificación es obligatorio")]
        public int TipoIdentificacion { get; set; }

        [Display(Name = "Número de Identificación")]
        [Required(ErrorMessage = "El numero de identificacion es obligatorio")]
        [MaxLength(20, ErrorMessage = "El número de identificación no puede exceder los 20 caracteres")]
        public string NumeroIdentificacion { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(50, ErrorMessage = "El primer apellido no puede exceder los 50 caracteres")]
        public string PrimerApellido { get; set; }

        [Display(Name = "Segundo Apellido")]
        [MaxLength(50, ErrorMessage = "El segundo apellido no puede exceder los 50 caracteres")]
        public string? SegundoApellido { get; set; }

        [Display(Name = "Correo Electrónico")]
        [MaxLength(150, ErrorMessage = "El correo electrónico no puede exceder los 150 caracteres")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string? CorreoElectronico { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El número de teléfono no puede exceder los 20 caracteres")]
        [Phone(ErrorMessage = "El número de teléfono no es válido")]
        public string? Telefono { get; set; }

        public List<SelectListItem> TiposIdentificacionList { get; set; } = new List<SelectListItem>();
    }
}
