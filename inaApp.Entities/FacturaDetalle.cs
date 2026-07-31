using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table("tbFacturaDetalle")]
    public class FacturaDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int FacturaId { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        //campos nuevos agregados
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PorcentajeImpuesto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoImpuesto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DescuentoAplicado { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }
        // Propiedades de navegación
        [ForeignKey("FacturaId")]
        public Factura Factura { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
    }
}