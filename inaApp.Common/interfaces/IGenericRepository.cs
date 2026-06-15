using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static inaApp.Common.Enums.Enumeradores;

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
        Task<E> ObtenerPorNombreAsync(string nombre);
        Task<bool> ExistePorIdentificacionAsync(TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null);
    }
}