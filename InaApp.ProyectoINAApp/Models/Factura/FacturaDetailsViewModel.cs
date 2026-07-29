using inaApp.Common.Enums;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    // Detalle de la factura para mostrar en la vista/boton de detalles
    public class FacturaDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteCedula { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteCorreo { get; set; }
        //campo agregado para mostrar el tipo de documento en la vista de detalles
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public string TipoDocumentoNombre => TipoDocumento switch
        {
            TipoDocumentoEnum.FacturaElectronica => "Factura Electrónica",
            TipoDocumentoEnum.NotaCreditoElectronica => "Nota de Crédito Electrónica",
            _ => TipoDocumento.ToString()
        };
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new List<FacturaDetalleViewModel>();
        //notas de credito asociadas a la factura
        public List<NotaCreditoIndexViewModel> NotasCredito { get; set; } = new List<NotaCreditoIndexViewModel>();
        public string EstadoTexto => Estado ? "Activa" : "Anulada";
        public string EstadoBadge => Estado ? "success" : "danger";
    }
}