using inaApp.Common.Enums;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public int CantidadProductos { get; set; }
        //campo agregado para mostrar el tipo de documento en la vista
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public string TipoDocumentoNombre => TipoDocumento switch
        {
            TipoDocumentoEnum.FacturaElectronica => "Factura Electrónica",
            TipoDocumentoEnum.NotaCreditoElectronica => "Nota de Crédito Electrónica",
            _ => TipoDocumento.ToString()
        };

        public string EstadoTexto => Estado ? "Activa" : "Anulada";
        public string EstadoBadge => Estado ? "success" : "danger";
    }
}