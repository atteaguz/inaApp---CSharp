namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class NotaCreditoDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int FacturaOriginalId { get; set; }
        public string FacturaOriginalNumero { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string Motivo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<NotaCreditoDetalleViewModel> Detalles { get; set; } = new List<NotaCreditoDetalleViewModel>();

        public string TipoDocumento => "Nota de Crédito Electrónica";
    }
}