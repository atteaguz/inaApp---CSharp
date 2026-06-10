using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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
            /***[reglas de negocio]***/

            //precio sea mayor a 0 - InvalidPriceException - BadRequest
            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0.");
            }
            //no nombres repetidos - DuplicatedNameException - BadRequest
            if (await _productoRepo.ObtenerPorNombreAsync(entity.Nombre) != null && entity.Id != entity.Id)
            {
                throw new DuplicatedNameException($"El nombre de producto: {entity.Nombre} , ya existe.");
            }
            //stock no negativo o 0 - InvalidStockException - BadRequest
            if (entity.Stock <= 0)
            {
                throw new InvalidStockException("El stock no puede ser negativo o cero.");
            }

            /*
            var producto = await _productoRepo.ObtenerTodosAsync();
            if (producto.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower() && p.Id != entity.Id))*/

             return await _productoRepo.ActualizarAsync(entity);
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            /***[reglas de negocio]***/

            //precio sea mayor a 0 - InvalidPriceException - BadRequest
            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0.");
            }
            //stock no negativo o 0 - InvalidStockException - BadRequest
            if (entity.Stock <= 0)
            {
                throw new InvalidStockException("El stock no puede ser negativo o cero.");
            }
            //no nombres repetidos - DuplicatedNameException - BadRequest
            if (await _productoRepo.ObtenerPorNombreAsync(entity.Nombre) != null)
            {
                throw new DuplicatedNameException($"El nombre de producto: {entity.Nombre} , ya existe.");
            }
            

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
                throw new NotFoundException($"Producto con id: {id} no encontrado.");
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
