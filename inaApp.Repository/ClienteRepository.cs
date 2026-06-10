using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {

        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //modificar cliente por id y que este activo
        public Task<Cliente> ActualizarAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        //crear cliente nuevo, activo por defecto
        public Task<Cliente> CrearAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        //eliminar cliente por id y que este activo - borrado logico
        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        //obtener cliente por id y que este activo
        public Task<Cliente> ObtenerPorIdsAsync(int id)
        {
            try
            {
                return _context.Cliente.AsNoTracking().Where(c => c.Id == id && c.Estado == true).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //obtener todos los clientes activos
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Cliente.AsNoTracking().Where(c => c.Estado == true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //obtener cliente por nombre y que este activo
        public async Task<Cliente> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                return await _context.Cliente.AsNoTracking().Where(c => c.Nombre == nombre && c.Estado == true).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
