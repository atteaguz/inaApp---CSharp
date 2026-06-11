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
        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            try
            {
                var cliente = await ObtenerPorIdsAsync(entity.IdCliente);
                if (cliente == null) return null;
                cliente.TipoIdentificacion = entity.TipoIdentificacion;
                cliente.NumeroIdentificacion = entity.NumeroIdentificacion;
                cliente.Nombre = entity.Nombre;
                cliente.PrimerApellido = entity.PrimerApellido;
                cliente.SegundoApellido = entity.SegundoApellido;
                cliente.CorreoElectronico = entity.CorreoElectronico;
                cliente.Telefono = entity.Telefono;
                _context.Cliente.Update(cliente);
                await _context.SaveChangesAsync();
                return cliente;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //crear cliente nuevo, activo por defecto
        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            try
            {
                await _context.Cliente.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //eliminar cliente por id y que este activo - borrado logico
        public async Task<bool> EliminarAsync(int IdCliente)
        {
            try
            {
                var cliente = await ObtenerPorIdsAsync(IdCliente);
                if (cliente == null) return false;

                //borrado logico
                cliente.Estado = false;

                _context.Cliente.Update(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //obtener cliente por id y que este activo
        public Task<Cliente> ObtenerPorIdsAsync(int IdCliente)
        {
            try
            {
                return _context.Cliente.AsNoTracking().Where(c => c.IdCliente == IdCliente && c.Estado == true).SingleOrDefaultAsync();
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

        public async Task<bool> ExistePorIdentificacionAsync(byte tipoIdentificacion, string numeroIdentificacion, int? idExcluir = null)
        {
            var query = _context.Cliente
                .Where(c => c.TipoIdentificacion == tipoIdentificacion &&
                            c.NumeroIdentificacion == numeroIdentificacion);

            if (idExcluir.HasValue)
                query = query.Where(c => c.IdCliente != idExcluir.Value);

            return await query.AnyAsync();
        }
    }
}
