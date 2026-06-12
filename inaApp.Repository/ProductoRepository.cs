using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {

        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //metodo para actualizar un producto existente
        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            try
            {
                var producto = await ObtenerPorIdsAsync(entity.Id);
                if (producto == null) return null;
                //actualizar los campos del producto
                producto.Nombre = entity.Nombre;
                producto.Precio = entity.Precio;
                producto.Descripcion = entity.Descripcion;
                producto.Stock = entity.Stock;
                _context.Producto.Update(producto);
                await _context.SaveChangesAsync();
                return producto;

                /*
                _context.Producto.Update(entity);
                await _context.SaveChangesAsync();
                return entity;*/
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
                return await _context.Producto.AsNoTracking().Where(p => p.Id == id && p.Estado == true).SingleOrDefaultAsync();
                /*if (entity is null)
                {
                    throw new Exception("No se encontro la entidad");
                }
                return entity;*/
            }
            catch (DbUpdateException ex)
            {

                throw ex;
            }
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Producto.AsNoTracking().Where(p => p.Estado == true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //agregado por regla de negocio "validar nombres repetidos"
        public async Task<Producto> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                return await _context.Producto.AsNoTracking().Where(p => p.Nombre == nombre && p.Estado == true).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> ExistePorIdentificacionAsync(TipoIdentificacionEnum tipoIdentificacion, string numeroIdentificacion) //int? idExcluir = null)
        {
            throw new NotImplementedException();
        }
    }
}
