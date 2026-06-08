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

        public Task<Producto> ActualizarAsync(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> CrearAsync(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
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
