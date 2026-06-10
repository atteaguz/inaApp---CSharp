using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace inaApp.Entities
{
    [Table("tbCliente")]
    [Index(nameof(TipoIdentificacion), nameof(NumeroIdentificacion), IsUnique = true)]
    public class Cliente
    {
        //propiedades de cliente
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required]
        public byte TipoIdentificacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroIdentificacion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string PrimerApellido { get; set; }

        [MaxLength(50)]
        public string SegundoApellido { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [MaxLength(20)]
        [Phone]
        public string Telefono { get; set; }    

        [Required]
        public bool Estado { get; set; } = true;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
