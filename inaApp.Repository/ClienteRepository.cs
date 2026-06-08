using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {
        public Task<Cliente> ActualizarAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> CrearAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
