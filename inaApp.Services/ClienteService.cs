using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Services
{
    public class ClienteService : IClienteService
    {
        //inyeccion de ClienteRepository
        private readonly IClienteRepository _clienteRepo;
        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

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

        public Task<List<Cliente>> ObtenerPorIdsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> ObtenerTodosAsync()
        {
            _clienteRepo.ObtenerTodosAsync();
            return null;
        }
    }
}
