using AutoMapper;
using inaApp.Common.Enums;
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
    public class FacturaService
    {
        private readonly FacturaRepository _facturaRepo;
        private readonly FacturaDetalleRepository _detalleRepo;
        private readonly ClienteRepository _clienteRepo;
        private readonly ProductoRepository _productoRepo;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FacturaService(
            FacturaRepository facturaRepo,
            FacturaDetalleRepository detalleRepo,
            ClienteRepository clienteRepo,
            ProductoRepository productoRepo,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _facturaRepo = facturaRepo;
            _detalleRepo = detalleRepo;
            _clienteRepo = clienteRepo;
            _productoRepo = productoRepo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<FacturaListDTO>>> ObtenerTodosAsync()
        {
            var facturas = await _facturaRepo.ObtenerTodosAsync();

            if (!facturas.Any())
                throw new NotFoundException("No hay facturas registradas");

            var facturasDTO = _mapper.Map<List<FacturaListDTO>>(facturas);

            //calculo de productos por factura
            foreach (var factura in facturasDTO)
            {
                var facturaEntity = facturas.First(f => f.Id == factura.Id);
                factura.CantidadProductos = facturaEntity.FacturaDetalles?.Count ?? 0;
            }

            return new Response<List<FacturaListDTO>>
            {
                Data = facturasDTO,
                Message = "Facturas obtenidas correctamente",
                Success = true
            };
        }

        public async Task<Response<FacturaResponseDTO>> ObtenerPorIdsAsync(int id)
        {
            var factura = await _facturaRepo.ObtenerPorIdsAsync(id);

            if (factura == null)
                throw new NotFoundException($"Factura con ID {id} no encontrada");

            var facturaDTO = _mapper.Map<FacturaResponseDTO>(factura);

            return new Response<FacturaResponseDTO>
            {
                Data = facturaDTO,
                Message = "Factura obtenida correctamente",
                Success = true
            };
        }

        public async Task<Response<FacturaResponseDTO>> CrearAsync(FacturaCreateDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validar cliente
                var cliente = await _clienteRepo.ObtenerPorIdsAsync(dto.ClienteId);
                if (cliente == null)
                    throw new NotFoundException($"Cliente con ID {dto.ClienteId} no encontrado");

                if (!cliente.Estado)
                    throw new ClienteInactivoException($"El cliente '{cliente.Nombre} {cliente.PrimerApellido}' está inactivo");

                // Validar que la factura tenga al menos un detalle
                if (dto.Detalles == null || !dto.Detalles.Any())
                    throw new FacturaSinDetalleException("La factura debe contener al menos un producto");

                // Validar productos duplicados
                var productosIds = dto.Detalles.Select(d => d.ProductoId).ToList();
                if (productosIds.Count != productosIds.Distinct().Count())
                    throw new DuplicadoProductoException("No se puede agregar el mismo producto dos veces");

                // Procesar cada producto
                decimal subtotal = 0;
                decimal descuentoTotal = 0;
                decimal impuestoTotal = 0;
                var detalles = new List<FacturaDetalle>();
                var productosParaActualizar = new List<(int ProductoId, int Cantidad)>();

                foreach (var detalleDto in dto.Detalles)
                {
                    var producto = await _context.Producto
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == detalleDto.ProductoId);

                    if (producto == null)
                        throw new NotFoundException($"Producto con ID {detalleDto.ProductoId} no encontrado");

                    if (!producto.Estado)
                        throw new InvalidOperationException($"El producto '{producto.Nombre}' está inactivo");

                    if (detalleDto.Cantidad <= 0)
                        throw new InvalidOperationException("La cantidad debe ser mayor a 0");

                    if (detalleDto.Cantidad > producto.Stock)
                        throw new InsufficientStockException(
                            $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {detalleDto.Cantidad}");

                    // Calculos por productos
                    // 1. Precio unitario
                    detalleDto.PrecioUnitario = producto.Precio;

                    // 2. Subtotal de línea (sin descuento)
                    detalleDto.Subtotal = Math.Round(detalleDto.Cantidad * detalleDto.PrecioUnitario, 2, MidpointRounding.AwayFromZero);

                    // 3. Descuento de línea
                    decimal descuentoAplicado = Math.Round(detalleDto.Subtotal * (producto.DescuentoMaximo / 100), 2, MidpointRounding.AwayFromZero);

                    // 4. Subtotal con descuento
                    decimal subtotalConDescuento = Math.Round(detalleDto.Subtotal - descuentoAplicado, 2, MidpointRounding.AwayFromZero);

                    // 5. Impuesto de línea
                    detalleDto.PorcentajeImpuesto = producto.PorcentajeImpuesto;
                    detalleDto.MontoImpuesto = Math.Round(subtotalConDescuento * (producto.PorcentajeImpuesto / 100), 2, MidpointRounding.AwayFromZero);

                    // 6. Total de línea
                    detalleDto.TotalLinea = Math.Round(subtotalConDescuento + detalleDto.MontoImpuesto, 2, MidpointRounding.AwayFromZero);

                    // 7. Sumar a totales de la factura
                    subtotal += detalleDto.Subtotal;
                    descuentoTotal += descuentoAplicado;
                    impuestoTotal += detalleDto.MontoImpuesto;

                    // Guardar el descuento aplicado en el detalle
                    detalleDto.DescuentoAplicado = descuentoAplicado;

                    // Crear el detalle de factura
                    var detalle = new FacturaDetalle
                    {
                        ProductoId = detalleDto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = detalleDto.PrecioUnitario,
                        Subtotal = detalleDto.Subtotal,
                        PorcentajeImpuesto = detalleDto.PorcentajeImpuesto,
                        MontoImpuesto = detalleDto.MontoImpuesto,
                        DescuentoAplicado = detalleDto.DescuentoAplicado,
                        TotalLinea = detalleDto.TotalLinea
                    };

                    detalles.Add(detalle);

                    productosParaActualizar.Add((producto.Id, detalleDto.Cantidad));
                }

                //actualizar stock de productos comprados
                foreach (var (productoId, cantidad) in productosParaActualizar)
                {
                    var productoToUpdate = await _context.Producto.FindAsync(productoId);
                    if (productoToUpdate == null)
                        throw new NotFoundException($"Producto con ID {productoId} no encontrado");

                    productoToUpdate.Stock -= cantidad;
                    if (productoToUpdate.Stock < 0)
                    {
                        throw new InvalidOperationException($"Stock insuficiente para el producto ID {productoId}. Stock resultante: {productoToUpdate.Stock}");
                    }
                }

                // Redondear totales finales de la factura
                subtotal = Math.Round(subtotal, 2, MidpointRounding.AwayFromZero);
                descuentoTotal = Math.Round(descuentoTotal, 2, MidpointRounding.AwayFromZero);
                impuestoTotal = Math.Round(impuestoTotal, 2, MidpointRounding.AwayFromZero);
                decimal totalFinal = Math.Round(subtotal - descuentoTotal + impuestoTotal, 2, MidpointRounding.AwayFromZero);

                // Crear la factura
                var factura = new Factura
                {
                    ClienteId = dto.ClienteId,
                    Fecha = dto.Fecha,
                    TipoDocumento = dto.TipoDocumento,
                    Subtotal = subtotal,
                    Descuento = descuentoTotal,
                    ImpuestoTotal = impuestoTotal,
                    Total = totalFinal,
                    Estado = true,
                    FechaCreacion = DateTime.Now,
                    FacturaDetalles = detalles
                };

                await _context.Factura.AddAsync(factura);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Cargar la factura creada para el response
                var facturaCreada = await _facturaRepo.ObtenerPorIdsAsync(factura.Id);
                var responseDTO = _mapper.Map<FacturaResponseDTO>(facturaCreada);

                return new Response<FacturaResponseDTO>
                {
                    Data = responseDTO,
                    Message = $"Factura #{factura.Id} creada exitosamente. Total: ₡{totalFinal:N2}",
                    Success = true
                };
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                var innerEx = ex.InnerException;
                var errorMessage = ex.Message;
                while (innerEx != null)
                {
                    errorMessage += $"\n  → Inner: {innerEx.Message}";
                    Console.WriteLine($"❌ Inner Exception: {innerEx.Message}");
                    innerEx = innerEx.InnerException;
                }
                Console.WriteLine($"❌ Error completo: {errorMessage}");
                throw new Exception($"Error al guardar la factura: {errorMessage}", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Error general: {ex.Message}");
                throw;
            }
        }

        public async Task<Response<bool>> AnularAsync(int id)
        {
            var factura = await _facturaRepo.ObtenerPorIdsAsync(id);

            if (factura == null)
                throw new NotFoundException($"Factura con ID {id} no encontrada");

            if (!factura.Estado)
                throw new InvalidOperationException($"La factura #{id} ya está anulada");

            // Verificar si tiene Notas de Crédito asociadas
            if (factura.NotasCredito != null && factura.NotasCredito.Any())
            {
                // Si tiene notas de crédito, no se puede anular directamente
                // Se debe usar el proceso de Nota de Crédito
                throw new InvalidOperationException(
                    $"La factura #{id} tiene notas de crédito asociadas. Use el proceso de Nota de Crédito para anular completamente.");
            }

            var result = await _facturaRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = result,
                Message = result ? $"Factura #{id} anulada correctamente" : "Error al anular la factura",
                Success = result
            };
        }
    }
}