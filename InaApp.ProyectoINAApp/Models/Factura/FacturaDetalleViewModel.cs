using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    //Detalle factura al momento de agregar productos a la factura
    public class FacturaDetalleViewModel
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int StockDisponible { get; set; }
    }
}