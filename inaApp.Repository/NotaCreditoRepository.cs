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
    public class NotaCreditoRepository : IGenericRepository<NotaCredito>
    {
        private readonly ApplicationDbContext _context;

        public NotaCreditoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotaCredito> CrearAsync(NotaCredito entity)
        {
            try
            {
                await _context.NotaCredito.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotaCredito> ActualizarAsync(NotaCredito entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<NotaCredito> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return await _context.NotaCredito
                    .AsNoTracking()
                    .Include(n => n.Cliente)
                    .Include(n => n.FacturaOriginal)
                    .Include(n => n.NotaCreditoDetalles)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(n => n.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NotaCredito>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.NotaCredito
                    .AsNoTracking()
                    .Include(n => n.Cliente)
                    .Include(n => n.FacturaOriginal)
                    .Include(n => n.NotaCreditoDetalles)
                    .OrderByDescending(n => n.FechaCreacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotaCredito> ObtenerPorNombreAsync(string nombre)
        {
            return null;
        }

        public async Task<bool> ExistePorIdentificacionAsync(Enumeradores.TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            return false;
        }

        public async Task<List<NotaCredito>> ObtenerPorFacturaAsync(int facturaId)
        {
            try
            {
                return await _context.NotaCredito
                    .AsNoTracking()
                    .Include(n => n.Cliente)
                    .Include(n => n.FacturaOriginal)
                    .Include(n => n.NotaCreditoDetalles)
                        .ThenInclude(d => d.Producto)
                    .Where(n => n.FacturaOriginalId == facturaId)
                    .OrderByDescending(n => n.FechaCreacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}