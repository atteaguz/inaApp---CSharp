using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services
{
    public class ProductoService : IProductoService
    {

        private readonly IProductoRepository _productoRepo;

        public ProductoService(IProductoRepository productoRepo)
        {
            _productoRepo = productoRepo;
        }
        public Task<List<Producto>> ObtenerTodosAsync()
        {
            _productoRepo.ObtenerTodosAsync();
            return null;
        }
        public Task<List<Producto>> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Producto> CrearAsync(Producto producto)
        {
            throw new NotImplementedException();
        }
        public Task<Producto> ActualizarAsync(Producto producto)
        {
            throw new NotImplementedException();
        }
        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
