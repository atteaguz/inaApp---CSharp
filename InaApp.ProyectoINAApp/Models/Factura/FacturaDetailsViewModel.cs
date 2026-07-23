namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteCorreo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new List<FacturaDetalleViewModel>();

        public string EstadoTexto => Estado ? "Activa" : "Anulada";
        public string EstadoBadge => Estado ? "success" : "danger";
    }
}