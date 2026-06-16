using inaApp.Common.Enums;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Repository
{
    public class CategoriaRepository : IGenericRepository<Categoria>
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria> ActualizarAsync(Categoria entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Categoria> CrearAsync(Categoria entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Categoria> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Categoria> ObtenerPorNombreAsync(string nombre)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Categoria>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Categoria.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //no se usa
        public async Task<bool> ExistePorIdentificacionAsync(Enumeradores.TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            throw new NotImplementedException();
        }
    }
}
