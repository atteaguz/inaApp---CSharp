using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace inaApp.Services
{
    public class ProductoService : IGenericService
    <ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>
    {
        //inyeccion de ProductoRepository EN ProductoService
        private readonly IGenericRepository<Producto> _productoRepo;

        public ProductoService(IGenericRepository<Producto> productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public async Task<ProductoResponseDTO> ActualizarAsync(ProductoUpdateDTO entity)
        {
            //reglas de negocio

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

            var producto = await _productoRepo.ActualizarAsync(new Producto());

            return new ProductoResponseDTO();
        }

        public async Task<ProductoResponseDTO> CrearAsync(ProductoCreateDTO entity)
        {
            //reglas de negocio

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

            //converit DTO a entity y guardar en l BD
            var producto = await _productoRepo.CrearAsync(new Producto());

            //converir entity a DTO Response y retornar ProductoResponseDTO
            return new ProductoResponseDTO();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            //reglas de negocio

            var producto = await _productoRepo.ObtenerPorIdsAsync(id);
            if(producto == null || id <= 0)
            {
                throw new NotFoundException($"Error al eliminar: Producto con id: {id} no encontrado o nulo.");
            }

            return await _productoRepo.EliminarAsync(id);
        }

        public async Task<ProductoResponseDTO> ObtenerPorIdsAsync(int id)
        {
            //reglas de negocio

            var producto =  await _productoRepo.ObtenerPorIdsAsync(id);
            if (producto == null) 
            {
                throw new NotFoundException($"Producto con id: {id} no encontrado o nulo.");
            }

            return new ProductoResponseDTO();
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosAsync()
        {
            //reglas de negocio
            var lista = await _productoRepo.ObtenerTodosAsync();
            return new List<ProductoResponseDTO>();
        }
    }
}
