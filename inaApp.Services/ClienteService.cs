using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using inaApp.Common.Enums;

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
        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }

        //crear cliente, activo por defecto
        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            /***[reglas de negocio]***/

            //validar que el valor existe en el enum (1,2,3 o 4)
            if (!Enum.IsDefined(typeof(TipoIdentificacionEnum), entity.TipoIdentificacion))
            {
                throw new ArgumentException("El tipo de identificacion es inválido. Ingrese un valor valido.");
            }

            return await _clienteRepo.CrearAsync(entity);
        }

        //eliminar cliente por id - borrado logico
        public async Task<bool> EliminarAsync(int IdCliente)
        {
            //reglas de negocio

            var cliente = await _clienteRepo.ObtenerPorIdsAsync(IdCliente);
            if (cliente == null || IdCliente <= 0)
            {
                throw new NotFoundException($"Error al eliminar: Cliente con id: {IdCliente} no encontrado o nulo.");
            }
            return await _clienteRepo.EliminarAsync(IdCliente);
        }

        //obtener cliente por id y que este activo
        public async Task<Cliente> ObtenerPorIdsAsync(int IdCliente)
        {
            //reglas de negocio
            var cliente = await _clienteRepo.ObtenerPorIdsAsync(IdCliente);
            if (cliente == null)
            {
                throw new NotFoundException($"Cliente con id: {IdCliente} no encontrado.");
            }
            return cliente;
        }

        //obtener todos los clientes activos
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepo.ObtenerTodosAsync();
        }
    }
}
