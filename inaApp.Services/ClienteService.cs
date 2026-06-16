using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.Services
{
    public class ClienteService : IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>
    {
        //inyeccion de ClienteRepository
        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly IMapper _mapper;
        public ClienteService(IGenericRepository<Cliente> clienteRepo, IMapper mapper)
        {
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        //modificar cliente por id y que este activo
        public async Task<ClienteResponseDTO> ActualizarAsync(ClienteUpdateDTO entity)
        {
            //reglas de negocio

            //validar al cliente
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El cliente no puede ser nulo");

            if (entity.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente no es válido");

            //validar campos requeridos
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldMissingException("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(entity.PrimerApellido))
                throw new RequiredFieldMissingException("El primer apellido es obligatorio");

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
                throw new RequiredFieldMissingException("El número de identificación es obligatorio");

            //validar formato de correo si lo cambia
            if (!string.IsNullOrEmpty(entity.CorreoElectronico))
            {
                if (!IsValidEmail(entity.CorreoElectronico))
                    throw new InvalidEmailFormatException($"El formato del correo '{entity.CorreoElectronico}' no es válido");
            }

            //validar formato de teléfono si lo cambia
            if (!string.IsNullOrEmpty(entity.Telefono))
            {
                if (!IsValidPhone(entity.Telefono))
                    throw new InvalidPhoneFormatException($"El formato del teléfono '{entity.Telefono}' no es válido");
            }

            //si cambia la identificación, validar que no esté duplicada
            if (!Enum.IsDefined(typeof(TipoIdentificacionEnum), entity.TipoIdentificacion))
            {
                throw new InvalidIdentificationException("El tipo de identificación es inválido");
            }

            var existeCliente = await _clienteRepo.ExistePorIdentificacionAsync(
                entity.TipoIdentificacion,
                entity.NumeroIdentificacion,
                entity.IdCliente);

            if (existeCliente)
            {
                throw new DuplicateIdentificationException($"Ya existe otro cliente con tipo '{entity.TipoIdentificacion}' y número '{entity.NumeroIdentificacion}'");
            }

            var cliente = _mapper.Map<Cliente>(entity);
            cliente = await _clienteRepo.ActualizarAsync(new Cliente());

            var clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);
            return clienteResponse;
        }

        //crear cliente, activo por defecto
        public async Task<ClienteResponseDTO> CrearAsync(ClienteCreateDTO entity)
        {
            //reglas de negocio

            //validar campos requeridos
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El cliente no puede ser nulo");

            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldMissingException("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(entity.PrimerApellido))
                throw new RequiredFieldMissingException("El primer apellido es obligatorio");

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
                throw new RequiredFieldMissingException("El número de identificación es obligatorio");

            //validar formato de correo electrónico
            if (!string.IsNullOrEmpty(entity.CorreoElectronico))
            {
                if (!IsValidEmail(entity.CorreoElectronico))
                    throw new InvalidEmailFormatException($"El formato del correo '{entity.CorreoElectronico}' no es válido");
            }

            //validar formato de teléfono (si se proporciona)
            if (!string.IsNullOrEmpty(entity.Telefono))
            {
                if (!IsValidPhone(entity.Telefono))
                    throw new InvalidPhoneFormatException($"El formato del teléfono '{entity.Telefono}' no es válido.");
            }

            //validar tipo de identificacion
            if (!Enum.IsDefined(typeof(TipoIdentificacionEnum), entity.TipoIdentificacion))
            {
                throw new InvalidIdentificationException($"El tipo de identificación '{entity.TipoIdentificacion}' es inválido. Valores permitidos: 1 (Cédula Física), 2 (Cédula Jurídica), 3 (DIMEX), 4 (Pasaporte)");
            }
            //validar que no exista otro cliente con el mismo tipo de identificacion y numero de identificacion
            var existeCliente = await _clienteRepo.ExistePorIdentificacionAsync(
                entity.TipoIdentificacion,
                entity.NumeroIdentificacion/*,
                entity.IdCliente);*/);
            if (existeCliente)
            {
                throw new DuplicateIdentificationException($"Ya existe un cliente con tipo '{entity.TipoIdentificacion}' y número '{entity.NumeroIdentificacion}'");
            }

            Cliente cliente = _mapper.Map<Cliente>(entity);

            cliente = await _clienteRepo.CrearAsync(cliente);

            //converir entity a DTO Response y retornar ProductoResponseDTO
            ClienteResponseDTO clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);

            return clienteResponse;
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
        public async Task<ClienteResponseDTO> ObtenerPorIdsAsync(int IdCliente)
        {
            //reglas de negocio
            var cliente = await _clienteRepo.ObtenerPorIdsAsync(IdCliente);
            if (cliente == null)
            {
                throw new NotFoundException($"Cliente con id: {IdCliente} no encontrado.");
            }

            //convertir Entity a DTOResponse
            var clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);
            return clienteResponse;
        }

        //obtener todos los clientes activos
        public async Task<List<ClienteResponseDTO>> ObtenerTodosAsync()
        {
            var listaClientes = await _clienteRepo.ObtenerTodosAsync();

            //validar que la lista no este vacia
            if (!listaClientes.Any())
            {
                throw new NotFoundException("No se encontraron productos");
            }

            var listaDTOs = _mapper.Map<List<ClienteResponseDTO>>(listaClientes);

            return listaDTOs;
        }

        //mtodos auxiliares de validación
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) &&
                   Regex.IsMatch(phone, @"^\+?[\d\s\-]+$");
        }
    }
}
