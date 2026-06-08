using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.interfaces
{
    //interfaz generica para repositorios
    public interface IGenericRepository <E>
    {
        Task<List<E>> ObtenerTodosAsync();
        Task<E> ObtenerPorIdsAsync(int id);
        Task<E> CrearAsync(E entity);
        Task<E> ActualizarAsync(E entity);
        Task<bool> EliminarAsync(int id);
    }
}
