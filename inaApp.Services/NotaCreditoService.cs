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
                // 1. Validar que la factura original exista y esté activa
                var facturaOriginal = await _facturaRepo.ObtenerPorIdsAsync(dto.FacturaOriginalId);
                if (facturaOriginal == null)
                    throw new NotFoundException($"Factura original con ID {dto.FacturaOriginalId} no encontrada");

                if (!facturaOriginal.Estado)
                    throw new InvalidOperationException($"La factura #{dto.FacturaOriginalId} ya está anulada");

                // 2. Validar que el cliente esté activo
                var cliente = await _clienteRepo.ObtenerPorIdsAsync(facturaOriginal.ClienteId);
                if (cliente == null || !cliente.Estado)
                    throw new ClienteInactivoException("El cliente de la factura original no está activo");

                // 3. Validar que haya al menos un detalle
                if (dto.Detalles == null || !dto.Detalles.Any())
                    throw new FacturaSinDetalleException("La nota de crédito debe contener al menos un producto");

                // 4. Procesar cada detalle
                decimal subtotal = 0;
                decimal descuentoTotal = 0;
                decimal impuestoTotal = 0;
                var detallesNota = new List<NotaCreditoDetalle>();

                // Diccionario para mantener productos ya actualizados (evita duplicados)
                var productosActualizados = new Dictionary<int, Producto>();

                foreach (var detalleDto in dto.Detalles)
                {
                    // Buscar el detalle original de la factura
                    var detalleOriginal = facturaOriginal.FacturaDetalles
                        .FirstOrDefault(d => d.Id == detalleDto.FacturaDetalleOriginalId);

                    if (detalleOriginal == null)
                        throw new NotFoundException($"Detalle de factura con ID {detalleDto.FacturaDetalleOriginalId} no encontrado");

                    // Validar que la cantidad no exceda la cantidad original
                    if (detalleDto.Cantidad > detalleOriginal.Cantidad)
                        throw new InvalidOperationException(
                            $"La cantidad a acreditar ({detalleDto.Cantidad}) excede la cantidad facturada ({detalleOriginal.Cantidad})");

                    if (detalleDto.Cantidad <= 0)
                        throw new InvalidOperationException("La cantidad debe ser mayor a 0");

                    // Obtener el producto que ya viene en el detalle original (evita duplicados)
                    var producto = detalleOriginal.Producto;

                    // Si el producto es null (no se cargó en la consulta), obtenerlo separadamente
                    if (producto == null)
                    {
                        producto = await _context.Producto.FindAsync(detalleOriginal.ProductoId);
                        if (producto == null)
                            throw new NotFoundException($"Producto con ID {detalleOriginal.ProductoId} no encontrado");
                    }

                    // Verificar si este producto ya fue actualizado en esta misma nota
                    if (productosActualizados.ContainsKey(producto.Id))
                    {
                        // Usar el producto ya actualizado
                        producto = productosActualizados[producto.Id];
                    }
                    else
                    {
                        // Devolucion de stock
                        producto.Stock += detalleDto.Cantidad;
                        productosActualizados[producto.Id] = producto;

                        // Marcar como modificado para que EF lo actualice
                        _context.Entry(producto).State = EntityState.Modified;
                    }

                    // Copiar datos del detalle original
                    detalleDto.ProductoId = detalleOriginal.ProductoId;
                    detalleDto.PrecioUnitario = detalleOriginal.PrecioUnitario;
                    detalleDto.Subtotal = detalleDto.Cantidad * detalleDto.PrecioUnitario;
                    detalleDto.PorcentajeImpuesto = detalleOriginal.PorcentajeImpuesto;
                    detalleDto.MontoImpuesto = detalleDto.Subtotal * (detalleOriginal.PorcentajeImpuesto / 100);
                    detalleDto.DescuentoAplicado = detalleOriginal.DescuentoAplicado * (detalleDto.Cantidad / (decimal)detalleOriginal.Cantidad);
                    detalleDto.TotalLinea = detalleDto.Subtotal - detalleDto.DescuentoAplicado + detalleDto.MontoImpuesto;
                    detalleDto.ProductoNombre = producto.Nombre;
                    detalleDto.CantidadOriginal = detalleOriginal.Cantidad;

                    // Sumar a totales
                    subtotal += detalleDto.Subtotal;
                    descuentoTotal += detalleDto.DescuentoAplicado;
                    impuestoTotal += detalleDto.MontoImpuesto;

                    // Crear detalle de nota de crédito
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

                // 5. Calcular totales de la nota de crédito
                decimal total = subtotal - descuentoTotal + impuestoTotal;

                // 6. Crear la nota de crédito
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

                // 7. Verificar si se anuló completamente la factura
                var detallesFactura = facturaOriginal.FacturaDetalles.ToList();
                bool todosAcreditados = true;

                foreach (var detalleFactura in detallesFactura)
                {
                    // Calcular cuánto se ha acreditado de este detalle en esta nota
                    var acreditadoEnEstaNota = detallesNota
                        .Where(d => d.FacturaDetalleOriginalId == detalleFactura.Id)
                        .Sum(d => d.Cantidad);

                    // Buscar si hay notas de crédito anteriores que ya acreditaron parte
                    var notasAnteriores = await _notaCreditoRepo.ObtenerPorFacturaAsync(facturaOriginal.Id);
                    var acreditadoAnterior = 0;
                    foreach (var notaAnterior in notasAnteriores)
                    {
                        if (notaAnterior.Id != notaCredito.Id)
                        {
                            acreditadoAnterior += notaAnterior.NotaCreditoDetalles
                                .Where(d => d.FacturaDetalleOriginalId == detalleFactura.Id)
                                .Sum(d => d.Cantidad);
                        }
                    }

                    var totalAcreditado = acreditadoAnterior + acreditadoEnEstaNota;
                    if (totalAcreditado < detalleFactura.Cantidad)
                    {
                        todosAcreditados = false;
                        break;
                    }
                }

                // Si todos los productos fueron acreditados, anular la factura completa
                if (todosAcreditados)
                {
                    facturaOriginal.Estado = false;
                    _context.Factura.Update(facturaOriginal);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                // Cargar la nota de crédito creada para el response
                var notaCreada = await _notaCreditoRepo.ObtenerPorIdsAsync(notaCredito.Id);
                var responseDTO = _mapper.Map<NotaCreditoResponseDTO>(notaCreada);

                return new Response<NotaCreditoResponseDTO>
                {
                    Data = responseDTO,
                    Message = $"Nota de crédito #{notaCredito.Id} creada exitosamente. Stock actualizado. Total acreditable: ₡{total:N2}",
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