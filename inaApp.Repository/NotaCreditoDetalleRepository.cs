using inaApp.Common.Enums;
using inaApp.Common.interfaces;
using inaApp.Entities;
using inaApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class NotaCreditoDetalleRepository : IGenericRepository<NotaCreditoDetalle>
    {
        private readonly ApplicationDbContext _context;

        public NotaCreditoDetalleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotaCreditoDetalle> CrearAsync(NotaCreditoDetalle entity)
        {
            try
            {
                await _context.NotaCreditoDetalle.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotaCreditoDetalle> ActualizarAsync(NotaCreditoDetalle entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<NotaCreditoDetalle> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return await _context.NotaCreditoDetalle
                    .AsNoTracking()
                    .Include(d => d.Producto)
                    .Include(d => d.FacturaDetalleOriginal)
                    .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NotaCreditoDetalle>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.NotaCreditoDetalle
                    .AsNoTracking()
                    .Include(d => d.Producto)
                    .Include(d => d.FacturaDetalleOriginal)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotaCreditoDetalle> ObtenerPorNombreAsync(string nombre)
        {
            return null;
        }

        public async Task<bool> ExistePorIdentificacionAsync(Enumeradores.TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            return false;
        }
    }
}