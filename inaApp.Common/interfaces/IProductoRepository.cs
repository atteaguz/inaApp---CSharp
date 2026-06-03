using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Common.interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<List<Producto>> ObtenerPorIdsAsync(int id);
        Task<Producto> CrearAsync(Producto producto);
        Task<Producto> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(int id);
    }
}
