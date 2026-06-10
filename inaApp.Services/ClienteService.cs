using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services
{
    public class ClienteService : IGenericService<Cliente>
    {
        //inyeccion de ClienteRepository
        private readonly IGenericRepository<Cliente> _clienteRepo;
        public ClienteService(IGenericRepository<Cliente> clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        //modificar cliente por id y que este activo
        public Task<Cliente> ActualizarAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        //crear cliente, activo por defecto
        public Task<Cliente> CrearAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        //eliminar cliente por id - borrado logico
        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        //obtener cliente por id y que este activo
        public Task<Cliente> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        //obtener todos los clientes activos
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepo.ObtenerTodosAsync();
        }
    }
}
