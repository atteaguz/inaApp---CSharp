using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepo, IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _productoRepo = productoRepo;
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        public async Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO entity)
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
            //if (await _productoRepo.ObtenerPorNombreAsync(entity.Nombre) != null && entity.Id != entity.Id)
            //{
            //    throw new DuplicatedNameException($"El nombre de producto: {entity.Nombre} , ya existe.");
            //}

            //no nombre repetidos, se excluye a si mismo al actualizar si nombre no cambia - DuplicatedNameException - BadRequest
            var productoExistente = await _productoRepo.ObtenerPorIdsAsync(entity.Id);
            if (productoExistente == null)
                throw new NotFoundException($"Producto con ID {entity.Id} no encontrado.");

            var productoConMismoNombre = await _productoRepo.ObtenerPorNombreAsync(entity.Nombre);
            if (productoConMismoNombre != null && productoConMismoNombre.Id != entity.Id)
                throw new DuplicatedNameException($"El nombre de producto: {entity.Nombre} ya existe.");

            //categoria existe al actualizar
            if (entity.CategoriaId != productoExistente.CategoriaId)
            {
                var categoria = await _categoriaRepo.ObtenerPorIdsAsync(entity.CategoriaId);
                if (categoria == null)
                    throw new NotFoundException($"La categoría con ID {entity.CategoriaId} no existe.");
            }

            //mapeo de DTO a entity
            var producto = _mapper.Map<Producto>(entity);
            producto = await _productoRepo.ActualizarAsync(producto);

            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto Actualizado",
                Success = true
            }; 
        }

        public async Task<Response<ProductoResponseDTO>> CrearAsync(ProductoCreateDTO entity)
        {
            //reglas de negocio

            //nombre es obligatorio - RequiredFieldMissingException - BadRequest
            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new RequiredFieldMissingException("El nombre del producto es obligatorio");
            }

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

            //categoria debe existir
            var categoria = await _categoriaRepo.ObtenerPorIdsAsync(entity.CategoriaId);
            if (categoria == null)
            {
                throw new NotFoundException($"La categoría con ID {entity.CategoriaId} no existe.");
            }

            //categoria activa
            var categoriaActiva = await _categoriaRepo.ObtenerPorIdsAsync(entity.CategoriaId);
            if (categoriaActiva == null)
            {
                throw new NotFoundException($"La categoría con ID {entity.CategoriaId} no existe.");
            }
            if (!categoriaActiva.Estado)
            {
                throw new InvalidOperationException($"La categoría '{categoriaActiva.Nombre}' está inactiva y no se pueden asociar productos.");
            }            

            //convertir de DTO a Entidad
            Producto producto = _mapper.Map<Producto>(entity);

            producto = await _productoRepo.CrearAsync(producto);

            //converir entity a DTO Response y retornar ProductoResponseDTO

            return new Response<ProductoResponseDTO>
            { Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto Creado",
                Success = true,
            }; 
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            //reglas de negocio

            var producto = await _productoRepo.ObtenerPorIdsAsync(id);
            if(producto == null || id <= 0)
            {
                throw new NotFoundException($"Error al eliminar: Producto con id: {id} no encontrado o nulo.");
            }

            return new Response<bool>
            {
                Data = await _productoRepo.EliminarAsync(id),
                Message = "Producto Eliminado",
                Success = true,
            };
        }

        public async Task<Response<ProductoResponseDTO>> ObtenerPorIdsAsync(int id)
        {
            //reglas de negocio

            var producto =  await _productoRepo.ObtenerPorIdsAsync(id);
            if (producto == null) 
            {
                throw new NotFoundException($"Producto con id: {id} no encontrado o nulo.");
            }

            //convertir Entity a DTOResponse
            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto Obtenido",
                Success = true
            };
        }

        public async Task<Response<List<ProductoResponseDTO>>> ObtenerTodosAsync()
        {
            //reglas de negocio
            var listaProductos = await _productoRepo.ObtenerTodosAsync();

            //validar que la lista no este vacia
            if(!listaProductos.Any())
            {
                throw new NotFoundException("No se encontraron productos");
            }

            return new Response<List<ProductoResponseDTO>>
            { Data = _mapper.Map<List<ProductoResponseDTO>>(listaProductos),
            Message = "Productos Obtenidos",
            Success = true
            };
        }
    }
}
