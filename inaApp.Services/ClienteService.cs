using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static inaApp.Common.Enums.Enumeradores;

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

            //registrar fecha de actualización
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;

            return await _clienteRepo.ActualizarAsync(entity);
        }

        //crear cliente, activo por defecto
        public async Task<Cliente> CrearAsync(Cliente entity)
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
            // 6. Validar que no exista otro cliente con la misma identificación
            var existeCliente = await _clienteRepo.ExistePorIdentificacionAsync(
                entity.TipoIdentificacion,
                entity.NumeroIdentificacion,
                entity.IdCliente);
            if (existeCliente)
            {
                throw new DuplicateIdentificationException($"Ya existe un cliente con tipo '{entity.TipoIdentificacion}' y número '{entity.NumeroIdentificacion}'");
            }

            //asignar valores por defecto
            entity.Estado = true;
            entity.FechaCreacion = DateTime.Now;

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
