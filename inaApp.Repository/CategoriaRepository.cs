using inaApp.Common.Enums;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class CategoriaRepository : IGenericRepository<Categoria>
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Categoria
                    .AsNoTracking()
                    .Include(c => c.Productos)
                    .Where(p => p.Estado == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Categoria> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return await _context.Categoria
                    .AsNoTracking()
                    .Include(c => c.Productos)
                    .Where(p => p.Id == id && p.Estado == true)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Categoria> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                return await _context.Categoria
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Categoria> CrearAsync(Categoria entity)
        {
            try
            {
                await _context.Categoria.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Categoria> ActualizarAsync(Categoria entity)
        {
            try
            {
                var categoria = await ObtenerPorIdsAsync(entity.Id);
                if (categoria == null) return null;

                categoria.Nombre = entity.Nombre;

                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var categoria = await ObtenerPorIdsAsync(id);
                if (categoria == null) return false;

                //borrado logico
                categoria.Estado = false;

                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //no se usa en Categoria
        public Task<bool> ExistePorIdentificacionAsync(Enumeradores.TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            throw new NotImplementedException();
        }
    }
}