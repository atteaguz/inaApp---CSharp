using System.ComponentModel.DataAnnotations;


namespace InaApp.ProyectoINAApp.Models.Categoria
{
    public class CategoriaCreateViewModel
    {
        [Display(Name = "Nombre de la Categoría")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; }

        // relacion entre Categoria y Producto - 1 a *
        //public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
