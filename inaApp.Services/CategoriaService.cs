using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.Categoria;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class CategoriaService : IGenericService
        <CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>
    {
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<CategoriaResponseDTO>>> ObtenerTodosAsync()
        {
            var listaCategorias = await _categoriaRepo.ObtenerTodosAsync();

            if (!listaCategorias.Any())
            {
                throw new NotFoundException("No se encontraron categorias");
            }

            return new Response<List<CategoriaResponseDTO>>
            {
                Data = _mapper.Map<List<CategoriaResponseDTO>>(listaCategorias),
                Message = "Categorias obtenidas",
                Success = true
            };
        }

        public async Task<Response<CategoriaResponseDTO>> ObtenerPorIdsAsync(int id)
        {
            var categoria = await _categoriaRepo.ObtenerPorIdsAsync(id);

            if (categoria == null)
            {
                throw new NotFoundException($"Categoria con ID {id} no encontrada");
            }

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Message = "Categoria obtenida",
                Success = true
            };
        }

        public async Task<Response<CategoriaResponseDTO>> CrearAsync(CategoriaCreateDTO entity)
        {
            //nombre es obligatorio
            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new RequiredFieldMissingException("El nombre de la categoria es obligatorio");
            }

            //nombre debe ser unico
            var existe = await _categoriaRepo.ObtenerPorNombreAsync(entity.Nombre);
            if (existe != null)
            {
                throw new DuplicatedNameException($"Ya existe una categoria con el nombre '{entity.Nombre}'");
            }

            var categoria = _mapper.Map<Categoria>(entity);
            categoria = await _categoriaRepo.CrearAsync(categoria);

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Message = "Categoria creada exitosamente",
                Success = true
            };
        }

        public async Task<Response<CategoriaResponseDTO>> ActualizarAsync(CategoriaUpdateDTO entity)
        {
            //no permitir actualizar categorias inexistentes
            var categoriaExistente = await _categoriaRepo.ObtenerPorIdsAsync(entity.Id);
            if (categoriaExistente == null)
            {
                throw new NotFoundException($"Categoria con ID {entity.Id} no encontrada");
            }

            //nombre es obligatorio
            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new RequiredFieldMissingException("El nombre de la categoria es obligatorio");
            }

            //nombre es unico y se excluye a la misma categoria modificada por si no se modifica el nombre
            var existe = await _categoriaRepo.ObtenerPorNombreAsync(entity.Nombre);
            if (existe != null && existe.Id != entity.Id)
            {
                throw new DuplicatedNameException($"Ya existe otra categoria con el nombre '{entity.Nombre}'");
            }

            var categoria = _mapper.Map<Categoria>(entity);
            categoria = await _categoriaRepo.ActualizarAsync(categoria);

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Message = "Categoria actualizada exitosamente",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            //no permitir eliminar categorias inexistentes
            var categoria = await _categoriaRepo.ObtenerPorIdsAsync(id);
            if (categoria == null)
            {
                throw new NotFoundException($"Categoria con ID {id} no encontrada");
            }

            var result = await _categoriaRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = result,
                Message = result ? "Categoria eliminada exitosamente" : "Error al eliminar la categoria",
                Success = result
            };
        }
    }
}