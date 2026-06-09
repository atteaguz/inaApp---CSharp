using inaApp.Common.Exceptions;
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

        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            //reglas de negocio
            return await _productoRepo.ActualizarAsync(entity);
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

        public async Task<Producto> ObtenerPorIdsAsync(int id)
        {
            //reglas de negocio

            var pro =  await _productoRepo.ObtenerPorIdsAsync(id);
            if (pro == null) 
            {
                throw new NotFoundException($"Producto con id: {id} no encontrado. Existe?. Esta activo?");
            }

            return pro;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            //reglas de negocio
            return await _productoRepo.ObtenerTodosAsync();
        }
    }
}
