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

        private const decimal PORCENTAJE_DESCUENTO = 0.05m; //5%

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
                //validar cliente
                var cliente = await _clienteRepo.ObtenerPorIdsAsync(dto.ClienteId);
                if (cliente == null)
                    throw new NotFoundException($"Cliente con ID {dto.ClienteId} no encontrado");

                if (!cliente.Estado)
                    throw new ClienteInactivoException($"El cliente '{cliente.Nombre} {cliente.PrimerApellido}' está inactivo");

                //validar que la factura tenga al menos un detalle
                if (dto.Detalles == null || !dto.Detalles.Any())
                    throw new FacturaSinDetalleException("La factura debe contener al menos un producto");

                //validar productos duplicados
                var productosIds = dto.Detalles.Select(d => d.ProductoId).ToList();
                if (productosIds.Count != productosIds.Distinct().Count())
                    throw new DuplicadoProductoException("No se puede agregar el mismo producto dos veces");

                //procesar cada producto
                var productos = new List<Producto>();
                decimal subtotal = 0;
                decimal descuentoTotal = 0;
                decimal impuestoTotal = 0;
                var detalles = new List<FacturaDetalle>();

                foreach (var detalleDto in dto.Detalles)
                {
                    var producto = await _productoRepo.ObtenerPorIdsAsync(detalleDto.ProductoId);
                    if (producto == null)
                        throw new NotFoundException($"Producto con ID {detalleDto.ProductoId} no encontrado");

                    if (!producto.Estado)
                        throw new InvalidOperationException($"El producto '{producto.Nombre}' está inactivo");

                    if (detalleDto.Cantidad <= 0)
                        throw new InvalidOperationException("La cantidad debe ser mayor a 0");

                    if (detalleDto.Cantidad > producto.Stock)
                        throw new InsufficientStockException(
                            $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {detalleDto.Cantidad}");

                    //calculos con impuesto y descuento

                    //precio unitario
                    detalleDto.PrecioUnitario = producto.Precio;

                    //subtotal de línea sin descuento
                    detalleDto.Subtotal = detalleDto.Cantidad * detalleDto.PrecioUnitario;

                    //validar descuento maximo permitido por producto
                    //si se aplica descuento global, verificar que no exceda el maximo permitido
                    decimal descuentoAplicado = 0;
                    if (dto.Descuento > 0)
                    {
                        //calcular descuento sobre el subtotal de la linea
                        decimal descuentoLinea = detalleDto.Subtotal * (dto.Descuento / 100);
                        decimal descuentoMaximoPermitido = detalleDto.Subtotal * (producto.DescuentoMaximo / 100);

                        if (descuentoLinea > descuentoMaximoPermitido)
                        {
                            //aplicar descuento máximo permitido
                            descuentoAplicado = descuentoMaximoPermitido;
                        }
                        else
                        {
                            descuentoAplicado = descuentoLinea;
                        }
                    }

                    //guardar el descuento aplicado en el detalle
                    detalleDto.DescuentoAplicado = descuentoAplicado;

                    //subtotal despues de descuento para calcular impuesto
                    decimal subtotalConDescuento = detalleDto.Subtotal - descuentoAplicado;

                    //calcular impuesto
                    detalleDto.PorcentajeImpuesto = producto.PorcentajeImpuesto;
                    detalleDto.MontoImpuesto = subtotalConDescuento * (producto.PorcentajeImpuesto / 100);

                    //total de linea
                    detalleDto.TotalLinea = subtotalConDescuento + detalleDto.MontoImpuesto;

                    //sumar totales de la factura
                    subtotal += detalleDto.Subtotal;
                    descuentoTotal += descuentoAplicado;
                    impuestoTotal += detalleDto.MontoImpuesto;

                    //crear el detalle de factura
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

                    //actualizar los stock
                    producto.Stock -= detalleDto.Cantidad;
                    _context.Producto.Update(producto);

                    productos.Add(producto);
                }

                //calcular totales finales de la factura
                //total final = subtotal - descuento + impuestos
                decimal totalFinal = subtotal - descuentoTotal + impuestoTotal;

                //crear la factura
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

                //cargar la factura creada para el response
                var facturaCreada = await _facturaRepo.ObtenerPorIdsAsync(factura.Id);
                var responseDTO = _mapper.Map<FacturaResponseDTO>(facturaCreada);

                return new Response<FacturaResponseDTO>
                {
                    Data = responseDTO,
                    Message = $"Factura #{factura.Id} creada exitosamente. Total: ₡{totalFinal:N2}",
                    Success = true
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
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

        public async Task<Response<decimal>> CalcularTotalesAsync(FacturaCreateDTO dto)
        {
            decimal subtotal = 0;

            foreach (var detalle in dto.Detalles)
            {
                var producto = await _productoRepo.ObtenerPorIdsAsync(detalle.ProductoId);
                if (producto == null)
                    throw new NotFoundException($"Producto con ID {detalle.ProductoId} no encontrado");

                detalle.PrecioUnitario = producto.Precio;
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                subtotal += detalle.Subtotal;
            }

            var descuento = Math.Round(subtotal * PORCENTAJE_DESCUENTO, 2);
            var total = Math.Round(subtotal - descuento, 2);

            dto.Subtotal = subtotal;
            dto.Descuento = descuento;
            dto.Total = total;

            return new Response<decimal>
            {
                Data = total,
                Message = "Totales calculados correctamente",
                Success = true
            };
        }
    }
}