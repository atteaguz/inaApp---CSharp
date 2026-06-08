using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services
{
    public class ProductoService : IGenericService<Producto>
    {
        //inyeccion de ProductoRepository EN ProductoService
        private readonly IGenericRepository<Producto> _productoRepo;

        public ProductoService(IGenericRepository<Producto> productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public Task<Producto> ActualizarAsync(Producto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            //reglas de negocio
            return await _productoRepo.CrearAsync(entity);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            //reglas de negocio
            return await _productoRepo.EliminarAsync(id);
        }

        public Task<List<Producto>> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productoRepo.ObtenerTodosAsync();
        }
    }
}
