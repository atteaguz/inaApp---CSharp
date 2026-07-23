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
    public class FacturaRepository : IGenericRepository<Factura>
    {
        private readonly ApplicationDbContext _context;

        public FacturaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Factura> CrearAsync(Factura entity)
        {
            try
            {
                await _context.Factura.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Factura> ActualizarAsync(Factura entity)
        {
            try
            {
                var factura = await ObtenerPorIdsAsync(entity.Id);
                if (factura == null) return null;

                factura.Subtotal = entity.Subtotal;
                factura.Descuento = entity.Descuento;
                factura.Total = entity.Total;

                _context.Factura.Update(factura);
                await _context.SaveChangesAsync();
                return factura;
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
                var factura = await ObtenerPorIdsAsync(id);
                if (factura == null) return false;

                //anular la factura (borrado logico)
                factura.Estado = false;
                _context.Factura.Update(factura);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Factura> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return await _context.Factura
                    .AsNoTracking()
                    .Include(f => f.Cliente)
                    .Include(f => f.FacturaDetalles)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(f => f.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Factura>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Factura
                    .AsNoTracking()
                    .Include(f => f.Cliente)
                    .Include(f => f.FacturaDetalles)
                    .OrderByDescending(f => f.FechaCreacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //no se usa en factura
        public async Task<bool> ExistePorNombreAsync(string nombre)
        { 
            return false;
        }

        public async Task<List<Factura>> ObtenerPorClienteAsync(int clienteId)
        {
            try
            {
                return await _context.Factura
                    .AsNoTracking()
                    .Include(f => f.Cliente)
                    .Include(f => f.FacturaDetalles)
                    .Where(f => f.ClienteId == clienteId)
                    .OrderByDescending(f => f.FechaCreacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //no se usa en factura
        public async Task<Factura> ObtenerPorNombreAsync(string nombre)
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