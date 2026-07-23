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
    public class FacturaDetalleRepository : IGenericRepository<FacturaDetalle>
    {
        private readonly ApplicationDbContext _context;

        public FacturaDetalleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FacturaDetalle> CrearAsync(FacturaDetalle entity)
        {
            try
            {
                await _context.FacturaDetalle.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FacturaDetalle> ActualizarAsync(FacturaDetalle entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<FacturaDetalle> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return await _context.FacturaDetalle
                    .AsNoTracking()
                    .Include(d => d.Producto)
                    .Include(d => d.Factura)
                    .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FacturaDetalle>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.FacturaDetalle
                    .AsNoTracking()
                    .Include(d => d.Producto)
                    .Include(d => d.Factura)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistePorNombreAsync(string nombre)
        {
            return false;
        }

        public async Task<List<FacturaDetalle>> ObtenerPorFacturaAsync(int facturaId)
        {
            try
            {
                return await _context.FacturaDetalle
                    .AsNoTracking()
                    .Include(d => d.Producto)
                    .Where(d => d.FacturaId == facturaId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExisteProductoEnFacturaAsync(int facturaId, int productoId)
        {
            try
            {
                return await _context.FacturaDetalle
                    .AnyAsync(d => d.FacturaId == facturaId && d.ProductoId == productoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //no se usa en factura
        public async Task<FacturaDetalle> ObtenerPorNombreAsync(string nombre)
        {
            return null;
        }

        //no se usa en factura
        public async Task<bool> ExistePorIdentificacionAsync(Enumeradores.TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            return false;
        }
    }
}