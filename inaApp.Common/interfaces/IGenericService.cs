using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.interfaces
{
    //parametrizar una interfaz generica para que pueda ser reutilizada con cualquier entidad
    public interface IGenericService <E>
    {
        Task<List<E>> ObtenerTodosAsync();
        Task<E> ObtenerPorIdsAsync(int id);
        Task<E> CrearAsync(E entity);
        Task<E> ActualizarAsync(E entity);
        Task<bool> EliminarAsync(int id);
    }
}
