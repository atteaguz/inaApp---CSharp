namespace inaApp.DTOs.Factura
{
    public class FacturaDetalleResponseDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        //campos agregados
        public decimal PorcentajeImpuesto { get; set; }
        public decimal MontoImpuesto { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal TotalLinea { get; set; }
    }
}