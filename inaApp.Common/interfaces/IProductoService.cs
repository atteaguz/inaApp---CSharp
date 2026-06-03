using System;
using System.Collections.Generic;
using System.Text;
using inaApp.Entities;

namespace inaApp.Common.interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<List<Producto>> ObtenerPorIdsAsync(int id);
        Task<Producto> CrearAsync(Producto producto);
        Task<Producto> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(int id);

    }
}
