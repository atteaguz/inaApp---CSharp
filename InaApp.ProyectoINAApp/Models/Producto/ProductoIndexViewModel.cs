using inaApp.Common.Enums;

namespace InaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        //campos agregados para mostrar el tipo de impuesto y el porcentaje de impuesto
        public TipoImpuestoEnum TipoImpuesto { get; set; }
        public string TipoImpuestoNombre => TipoImpuesto switch
        {
            TipoImpuestoEnum.Exento => "Exento",
            TipoImpuestoEnum.IVA => "IVA",
            TipoImpuestoEnum.Selectivo => "Selectivo",
            _ => TipoImpuesto.ToString()
        };
        public decimal PorcentajeImpuesto { get; set; }
        public decimal DescuentoMaximo { get; set; }
    }
}
