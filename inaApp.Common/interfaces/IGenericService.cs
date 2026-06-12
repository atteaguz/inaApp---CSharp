using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.interfaces
{
    //parametrizar una interfaz generica para que pueda ser reutilizada con cualquier entidad
    public interface IGenericService <TResponse, TCreate, TUpdate>
    {
        Task<List<TResponse>> ObtenerTodosAsync();
        Task<TResponse> ObtenerPorIdsAsync(int id);
        Task<TResponse> CrearAsync(TCreate entity);
        Task<TResponse> ActualizarAsync(TUpdate entity);
        Task<bool> EliminarAsync(int id);
    }
}
