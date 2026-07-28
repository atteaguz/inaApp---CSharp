using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inaApp.Common.Enums;

namespace inaApp.Entities
{
    [Table("tbNotaCredito")]
    public class NotaCredito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FacturaOriginalId { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "El motivo no puede exceder los 1000 caracteres")]
        public string Motivo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImpuestoTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //notas de credito por default son TipoDocumento enum 2
        public TipoDocumentoEnum TipoDocumento => TipoDocumentoEnum.NotaCreditoElectronica;

        // Relaciones
        [ForeignKey("FacturaOriginalId")]
        public Factura FacturaOriginal { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        public ICollection<NotaCreditoDetalle> NotaCreditoDetalles { get; set; } = new List<NotaCreditoDetalle>();
    }
}