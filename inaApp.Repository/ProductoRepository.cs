using System;
using System.Collections.Generic;
using System.Text;
using inaApp.Common.interfaces;
using inaApp.Entities;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {
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

        public Task<List<Producto>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
