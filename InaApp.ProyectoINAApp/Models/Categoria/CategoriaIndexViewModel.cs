namespace InaApp.ProyectoINAApp.Models.Categoria
{
    public class CategoriaIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // relacion entre Categoria y Producto - 1 a *
        //public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
