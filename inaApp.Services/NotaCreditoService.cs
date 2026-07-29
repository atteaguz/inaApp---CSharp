using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Response;
using inaApp.Data;
using inaApp.DTOs.Factura;
using inaApp.Entities;
using inaApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class NotaCreditoService
    {
        private readonly NotaCreditoRepository _notaCreditoRepo;
        private readonly NotaCreditoDetalleRepository _notaCreditoDetalleRepo;
        private readonly FacturaRepository _facturaRepo;
        private readonly FacturaDetalleRepository _facturaDetalleRepo;
        private readonly ClienteRepository _clienteRepo;
        private readonly ProductoRepository _productoRepo;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotaCreditoService(
            NotaCreditoRepository notaCreditoRepo,
            NotaCreditoDetalleRepository notaCreditoDetalleRepo,
            FacturaRepository facturaRepo,
            FacturaDetalleRepository facturaDetalleRepo,
            ClienteRepository clienteRepo,
            ProductoRepository productoRepo,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _notaCreditoRepo = notaCreditoRepo;
            _notaCreditoDetalleRepo = notaCreditoDetalleRepo;
            _facturaRepo = facturaRepo;
            _facturaDetalleRepo = facturaDetalleRepo;
            _clienteRepo = clienteRepo;
            _productoRepo = productoRepo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<NotaCreditoResponseDTO>>> ObtenerTodosAsync()
        {
            var notas = await _notaCreditoRepo.ObtenerTodosAsync();

            if (!notas.Any())
                throw new NotFoundException("No hay notas de crédito registradas");

            var notasDTO = _mapper.Map<List<NotaCreditoResponseDTO>>(notas);

            return new Response<List<NotaCreditoResponseDTO>>
            {
                Data = notasDTO,
                Message = "Notas de credito obtenidas correctamente",
                Success = true
            };
        }

        public async Task<Response<NotaCreditoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var nota = await _notaCreditoRepo.ObtenerPorIdsAsync(id);

            if (nota == null)
                throw new NotFoundException($"Nota de credito con ID {id} no encontrada");

            var notaDTO = _mapper.Map<NotaCreditoResponseDTO>(nota);

            return new Response<NotaCreditoResponseDTO>
            {
                Data = notaDTO,
                Message = "Nota de credito obtenida correctamente",
                Success = true
            };
        }

        public async Task<Response<NotaCreditoResponseDTO>> CrearAsync(NotaCreditoCreateDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //validar que la factura original exista y este activa
                var facturaOriginal = await _facturaRepo.ObtenerPorIdsAsync(dto.FacturaOriginalId);
                if (facturaOriginal == null)
                    throw new NotFoundException($"Factura original con ID {dto.FacturaOriginalId} no encontrada");

                if (!facturaOriginal.Estado)
                    throw new InvalidOperationException($"La factura #{dto.FacturaOriginalId} ya está anulada");

                //validar que el cliente este activo
                var cliente = await _clienteRepo.ObtenerPorIdsAsync(facturaOriginal.ClienteId);
                if (cliente == null || !cliente.Estado)
                    throw new ClienteInactivoException("El cliente de la factura original no está activo");

                //validar que haya al menos un detalle
                if (dto.Detalles == null || !dto.Detalles.Any())
                    throw new FacturaSinDetalleException("La nota de crédito debe contener al menos un producto");

                //validar cada detalle
                decimal subtotal = 0;
                decimal descuentoTotal = 0;
                decimal impuestoTotal = 0;
                var detallesNota = new List<NotaCreditoDetalle>();

                foreach (var detalleDto in dto.Detalles)
                {
                    //obtener el detalle original de la factura
                    var detalleOriginal = facturaOriginal.FacturaDetalles
                        .FirstOrDefault(d => d.Id == detalleDto.FacturaDetalleOriginalId);

                    if (detalleOriginal == null)
                        throw new NotFoundException($"Detalle de factura con ID {detalleDto.FacturaDetalleOriginalId} no encontrado");

                    //validar que la cantidad no exceda la cantidad original
                    if (detalleDto.Cantidad > detalleOriginal.Cantidad)
                        throw new InvalidOperationException(
                            $"La cantidad a acreditar ({detalleDto.Cantidad}) excede la cantidad facturada ({detalleOriginal.Cantidad})");

                    //validar que la cantidad sea mayor a 0
                    if (detalleDto.Cantidad <= 0)
                        throw new InvalidOperationException("La cantidad debe ser mayor a 0");

                    //obtener el producto
                    var producto = await _productoRepo.ObtenerPorIdsAsync(detalleOriginal.ProductoId);
                    if (producto == null)
                        throw new NotFoundException($"Producto con ID {detalleOriginal.ProductoId} no encontrado");

                    //copia de los datos del detalle original
                    detalleDto.ProductoId = detalleOriginal.ProductoId;
                    detalleDto.PrecioUnitario = detalleOriginal.PrecioUnitario;
                    detalleDto.Subtotal = detalleDto.Cantidad * detalleDto.PrecioUnitario;
                    detalleDto.PorcentajeImpuesto = detalleOriginal.PorcentajeImpuesto;
                    detalleDto.MontoImpuesto = detalleDto.Subtotal * (detalleOriginal.PorcentajeImpuesto / 100);
                    detalleDto.DescuentoAplicado = detalleOriginal.DescuentoAplicado * (detalleDto.Cantidad / (decimal)detalleOriginal.Cantidad);
                    detalleDto.TotalLinea = detalleDto.Subtotal - detalleDto.DescuentoAplicado + detalleDto.MontoImpuesto;
                    detalleDto.ProductoNombre = producto.Nombre;
                    detalleDto.CantidadOriginal = detalleOriginal.Cantidad;

                    //sumar los totales
                    subtotal += detalleDto.Subtotal;
                    descuentoTotal += detalleDto.DescuentoAplicado;
                    impuestoTotal += detalleDto.MontoImpuesto;

                    //crear detalle de nota de credito
                    var detalle = new NotaCreditoDetalle
                    {
                        FacturaDetalleOriginalId = detalleDto.FacturaDetalleOriginalId,
                        ProductoId = detalleDto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = detalleDto.PrecioUnitario,
                        Subtotal = detalleDto.Subtotal,
                        PorcentajeImpuesto = detalleDto.PorcentajeImpuesto,
                        MontoImpuesto = detalleDto.MontoImpuesto,
                        DescuentoAplicado = detalleDto.DescuentoAplicado,
                        TotalLinea = detalleDto.TotalLinea
                    };

                    detallesNota.Add(detalle);
                }

                //calcular totales de la nota de credito
                decimal total = subtotal - descuentoTotal + impuestoTotal;

                //crear la nota de credito
                var notaCredito = new NotaCredito
                {
                    FacturaOriginalId = dto.FacturaOriginalId,
                    Fecha = DateTime.Now,
                    ClienteId = facturaOriginal.ClienteId,
                    Motivo = dto.Motivo,
                    Subtotal = subtotal,
                    Descuento = descuentoTotal,
                    ImpuestoTotal = impuestoTotal,
                    Total = total,
                    FechaCreacion = DateTime.Now,
                    NotaCreditoDetalles = detallesNota
                };

                await _context.NotaCredito.AddAsync(notaCredito);
                await _context.SaveChangesAsync();

                //verificar si se anulo completamente la factura
                //verificar si todos los productos de la factura han sido acreditados
                var detallesFactura = facturaOriginal.FacturaDetalles.ToList();
                bool todosAcreditados = true;

                foreach (var detalleFactura in detallesFactura)
                {
                    //calcular cuánto se ha acreditado de este detalle en esta nota
                    var acreditadoEnEstaNota = detallesNota
                        .Where(d => d.FacturaDetalleOriginalId == detalleFactura.Id)
                        .Sum(d => d.Cantidad);

                    if (acreditadoEnEstaNota < detalleFactura.Cantidad)
                    {
                        todosAcreditados = false;
                        break;
                    }
                }

                //si todos los productos fueron acreditados se anula la factura original completa
                if (todosAcreditados)
                {
                    facturaOriginal.Estado = false;
                    _context.Factura.Update(facturaOriginal);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                //cargar la nota de crédito creada para el response
                var notaCreada = await _notaCreditoRepo.ObtenerPorIdsAsync(notaCredito.Id);
                var responseDTO = _mapper.Map<NotaCreditoResponseDTO>(notaCreada);

                return new Response<NotaCreditoResponseDTO>
                {
                    Data = responseDTO,
                    Message = $"Nota de credito #{notaCredito.Id} creada exitosamente. Total acreditable: ₡{total:N2}",
                    Success = true
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Response<List<NotaCreditoResponseDTO>>> ObtenerPorFacturaAsync(int facturaId)
        {
            var notas = await _notaCreditoRepo.ObtenerPorFacturaAsync(facturaId);

            if (!notas.Any())
                throw new NotFoundException($"No hay notas de credito para la factura #{facturaId}");

            var notasDTO = _mapper.Map<List<NotaCreditoResponseDTO>>(notas);

            return new Response<List<NotaCreditoResponseDTO>>
            {
                Data = notasDTO,
                Message = "Notas de credito obtenidas correctamente",
                Success = true
            };
        }
    }
}