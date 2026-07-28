using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.DTOs.Producto
{
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        //nuevos campos agregados
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
