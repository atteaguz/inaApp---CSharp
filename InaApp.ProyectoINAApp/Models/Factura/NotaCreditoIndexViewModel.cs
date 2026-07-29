namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class NotaCreditoIndexViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string Motivo { get; set; }
        public int FacturaOriginalId { get; set; }
        public string FacturaOriginalNumero { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public int CantidadProductos { get; set; }
    }
}