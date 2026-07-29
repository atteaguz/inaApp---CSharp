using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table("tbFactura")]
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public int ClienteId { get; set; }
        //campo nuevo para el tipo de documento
        [Required]
        public TipoDocumentoEnum TipoDocumento { get; set; } = TipoDocumentoEnum.FacturaElectronica;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImpuestoTotal { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        [Required]
        public bool Estado { get; set; } = true;
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        public ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
        public ICollection<NotaCredito> NotasCredito { get; set; } = new List<NotaCredito>();
    }
}