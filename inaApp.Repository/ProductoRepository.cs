using System;
using System.Collections.Generic;
using System.Text;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {

        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Producto> ActualizarAsync(Producto entity)
        {

            try
            {
                _context.Producto.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            try
            {
                await _context.Producto.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
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
                var producto = await ObtenerPorIdsAsync(id);
                if (producto == null) return false;

                //borrado logico
                producto.Estado = false;

                _context.Producto.Update(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Producto> ObtenerPorIdsAsync(int id)
        {
            try
            {
                var entity = await _context.Producto.Where(p => p.Id == id && p.Estado == true).SingleOrDefaultAsync();
                if (entity is null)
                {
                    throw new Exception("No se encontro la entidad");
                }
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Producto.Where(p => p.Estado == true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
