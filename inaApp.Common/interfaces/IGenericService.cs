using System;
using System.Collections.Generic;
using System.Text;
using inaApp.Common.Response;

namespace inaApp.Common.interfaces
{
    //parametrizar una interfaz generica para que pueda ser reutilizada con cualquier entidad
    public interface IGenericService <TResponse, TCreate, TUpdate>
    {
        Task<Response<List<TResponse>>> ObtenerTodosAsync();
        Task<Response<TResponse>> ObtenerPorIdsAsync(int id);
        Task<Response<TResponse>> CrearAsync(TCreate entity);
        Task<Response<TResponse>> ActualizarAsync(TUpdate entity);
        Task<Response<bool>> EliminarAsync(int id);
    }
}
