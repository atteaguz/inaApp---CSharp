using static inaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteIndexViewModel
    {
        public int IdCliente { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}