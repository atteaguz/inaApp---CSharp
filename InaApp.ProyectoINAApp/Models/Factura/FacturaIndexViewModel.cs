namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public int CantidadProductos { get; set; }
        public string EstadoTexto => Estado ? "Activa" : "Anulada";
        public string EstadoBadge => Estado ? "success" : "danger";
    }
}