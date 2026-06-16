using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services
{
    public class CategoriaService : IGenericService
        <CategoriaResponseDTO, CategoriaUpdateDTO, CategoriaCreateDTO>
    {

        private readonly IGenericRepository<Cliente> _categoriaRepo;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Cliente> categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        public async Task<Response<CategoriaResponseDTO>> ActualizarAsync(CategoriaCreateDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CategoriaResponseDTO>> CrearAsync(CategoriaUpdateDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CategoriaResponseDTO>> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<CategoriaResponseDTO>>> ObtenerTodosAsync()
        {
            //reglas de negocio
            var listaCategorias = await _categoriaRepo.ObtenerTodosAsync();

            //validar que la lista no este vacia
            if (!listaCategorias.Any())
            {
                throw new NotFoundException("No se encontraron categroias");
            }

            return new Response<List<CategoriaResponseDTO>>
            {
                Data = _mapper.Map<List<CategoriaResponseDTO>>(listaCategorias),
                Message = "Categorias Obtenidos",
                Success = true
            };
        }
    }
}
