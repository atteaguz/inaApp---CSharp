using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using inaApp.Common.Response;
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
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepo, IMapper mapper)
        {
            _productoRepo = productoRepo;
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
            //Producto producto = new Producto
            //{
            //    Nombre = entity.Nombre,
            //    Precio = entity.Precio,
            //    Descripcion = entity.Descripcion,
            //    Stock = entity.Stock,
            //    Estado = true
            //};

            //convertir de DTO a Entidad
            Producto producto = _mapper.Map<Producto>(entity);

            producto = await _productoRepo.CrearAsync(producto);

            //converir entity a DTO Response y retornar ProductoResponseDTO

            //{
            //    Id = producto.Id,
            //    Nombre = producto.Nombre,
            //    Precio = producto.Precio,
            //    Descripcion = producto.Descripcion,
            //    Stock = producto.Stock
            //};

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
