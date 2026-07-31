using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table("tbNotaCreditoDetalle")]
    public class NotaCreditoDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int NotaCreditoId { get; set; }

        [Required]
        public int FacturaDetalleOriginalId { get; set; }

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

        // Relaciones
        [ForeignKey("NotaCreditoId")]
        public NotaCredito NotaCredito { get; set; }

        [ForeignKey("FacturaDetalleOriginalId")]
        public FacturaDetalle FacturaDetalleOriginal { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
    }
}