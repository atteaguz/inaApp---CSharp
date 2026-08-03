using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.DTOs.Factura;
using inaApp.Services;
using InaApp.ProyectoINAApp.Models.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class NotaCreditoController : Controller
    {
        private readonly NotaCreditoService _notaCreditoService;
        private readonly FacturaService _facturaService;
        private readonly IMapper _mapper;

        public NotaCreditoController(
            NotaCreditoService notaCreditoService,
            FacturaService facturaService,
            IMapper mapper)
        {
            _notaCreditoService = notaCreditoService;
            _facturaService = facturaService;
            _mapper = mapper;
        }

        // GET: NotaCreditoController/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await _notaCreditoService.ObtenerTodosAsync();

                if (!response.Success || response.Data == null || !response.Data.Any())
                {
                    ViewBag.Message = "No hay notas de credito registradas.";
                    return View(new List<NotaCreditoIndexViewModel>());
                }

                var listaViewModel = _mapper.Map<List<NotaCreditoIndexViewModel>>(response.Data);
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                ViewBag.Message = "No hay notas de credito registradas.";
                return View(new List<NotaCreditoIndexViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al cargar la pagina: " + ex.Message;
                return View(new List<NotaCreditoIndexViewModel>());
            }
        }

        // GET: NotaCreditoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _notaCreditoService.ObtenerPorIdAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = _mapper.Map<NotaCreditoDetailsViewModel>(response.Data);
                return View(viewModel);
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al obtener los detalles de la nota de credito.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: NotaCreditoController/Create/5 (desde factura)
        public async Task<ActionResult> Create(int facturaId)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdsAsync(facturaId);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction("Index", "Factura");
                }

                var factura = response.Data;

                if (!factura.Estado)
                {
                    TempData["Error"] = $"La factura #{facturaId} ya está anulada";
                    return RedirectToAction("Index", "Factura");
                }

                var viewModel = new NotaCreditoCreateViewModel
                {
                    FacturaOriginalId = factura.Id,
                    FacturaOriginalNumero = factura.Id.ToString(),
                    ClienteNombre = factura.ClienteNombre,
                    ClienteCedula = factura.ClienteCedula
                };

                // Cargar los detalles de la factura para seleccionar
                foreach (var detalle in factura.Detalles)
                {
                    viewModel.Detalles.Add(new NotaCreditoDetalleCreateViewModel
                    {
                        FacturaDetalleOriginalId = detalle.Id,
                        ProductoId = detalle.ProductoId,
                        ProductoNombre = detalle.ProductoNombre,
                        CantidadOriginal = detalle.Cantidad,
                        CantidadAcreditar = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal,
                        PorcentajeImpuesto = detalle.PorcentajeImpuesto,
                        MontoImpuesto = detalle.MontoImpuesto,
                        DescuentoAplicado = detalle.DescuentoAplicado,
                        TotalLinea = detalle.TotalLinea,
                        Seleccionado = true
                    });
                }

                // Calcular totales iniciales
                CalcularTotalesNota(viewModel);

                return View(viewModel);
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Factura");
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al cargar la factura para la nota de crédito.";
                return RedirectToAction("Index", "Factura");
            }
        }

        // NotaCreditoController.cs - POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NotaCreditoCreateViewModel viewModel)
        {
            try
            {
                // Si no hay detalles, cargarlos desde la factura
                if (viewModel.Detalles == null || !viewModel.Detalles.Any())
                {
                    await CargarDatosFactura(viewModel);
                }

                // Validar motivo
                if (string.IsNullOrWhiteSpace(viewModel.Motivo))
                {
                    viewModel.Error = "El motivo es obligatorio";
                    await CargarDatosFactura(viewModel);
                    return View(viewModel);
                }

                // Obtener solo los detalles seleccionados
                var detallesSeleccionados = viewModel.Detalles
                    .Where(d => d.Seleccionado && d.CantidadAcreditar > 0)
                    .ToList();

                if (!detallesSeleccionados.Any())
                {
                    viewModel.Error = "Debe seleccionar al menos un producto para acreditar";
                    await CargarDatosFactura(viewModel);
                    return View(viewModel);
                }

                // Crear DTO
                var dto = new NotaCreditoCreateDTO
                {
                    FacturaOriginalId = viewModel.FacturaOriginalId,
                    Motivo = viewModel.Motivo
                };

                foreach (var detalle in detallesSeleccionados)
                {
                    dto.Detalles.Add(new NotaCreditoDetalleCreateDTO
                    {
                        FacturaDetalleOriginalId = detalle.FacturaDetalleOriginalId,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.CantidadAcreditar
                    });
                }

                var response = await _notaCreditoService.CrearAsync(dto);

                if (!response.Success)
                {
                    viewModel.Error = response.Message;
                    await CargarDatosFactura(viewModel);
                    return View(viewModel);
                }

                TempData["Mensaje"] = response.Message;
                return RedirectToAction("Details", "NotaCredito", new { id = response.Data.Id });
            }
            catch (DbUpdateException ex)
            {
                var innerEx = ex.InnerException;
                var errorMessage = "Error al guardar la nota de crédito: ";
                while (innerEx != null)
                {
                    if (innerEx.Message.Contains("out of range"))
                    {
                        errorMessage = "Error: El descuento o impuesto excede el límite permitido. Verifique los valores.";
                        break;
                    }
                    innerEx = innerEx.InnerException;
                }
                viewModel.Error = errorMessage;
                await CargarDatosFactura(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                viewModel.Error = $"Error al crear la nota de crédito: {ex.Message}";
                await CargarDatosFactura(viewModel);
                return View(viewModel);
            }
        }

        // POST: NotaCreditoController/RecalcularTotales para la vista
        [HttpPost]
        public IActionResult ActualizarTotales(NotaCreditoCreateViewModel viewModel)
        {
            CalcularTotalesNota(viewModel);
            return Json(new
            {
                subtotal = viewModel.Subtotal,
                descuento = viewModel.Descuento,
                impuestoTotal = viewModel.ImpuestoTotal,
                total = viewModel.Total
            });
        }

        private void CalcularTotalesNota(NotaCreditoCreateViewModel viewModel)
        {
            decimal subtotal = 0;
            decimal descuento = 0;
            decimal impuesto = 0;

            foreach (var detalle in viewModel.Detalles.Where(d => d.Seleccionado && d.CantidadAcreditar > 0))
            {
                //calcular subtotal de linea
                var subtotalLinea = detalle.CantidadAcreditar * detalle.PrecioUnitario;

                //calcular descuento de linea
                var descuentoLinea = detalle.DescuentoAplicado > 0
                    ? (subtotalLinea / detalle.Subtotal) * detalle.DescuentoAplicado
                    : 0;

                //calcular impuesto de linea
                var baseImpuesto = subtotalLinea - descuentoLinea;
                var impuestoLinea = baseImpuesto * (detalle.PorcentajeImpuesto / 100);

                subtotal += subtotalLinea;
                descuento += descuentoLinea;
                impuesto += impuestoLinea;
            }

            viewModel.Subtotal = Math.Round(subtotal, 2);
            viewModel.Descuento = Math.Round(descuento, 2);
            viewModel.ImpuestoTotal = Math.Round(impuesto, 2);
            viewModel.Total = Math.Round(subtotal - descuento + impuesto, 2);
        }

        //Método auxiliar para cargar datos de la factura original
        private async Task CargarDatosFactura(NotaCreditoCreateViewModel viewModel)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdsAsync(viewModel.FacturaOriginalId);
                if (response.Success)
                {
                    var factura = response.Data;
                    viewModel.FacturaOriginalNumero = factura.Id.ToString();
                    viewModel.ClienteNombre = factura.ClienteNombre;
                    viewModel.ClienteCedula = factura.ClienteCedula;

                    // Si no hay detalles en el viewModel, cargarlos desde la factura
                    // Si ya hay detalles, conservarlos (el usuario ya seleccionó algo)
                    if (viewModel.Detalles == null || !viewModel.Detalles.Any())
                    {
                        viewModel.Detalles = new List<NotaCreditoDetalleCreateViewModel>();
                        foreach (var detalle in factura.Detalles)
                        {
                            viewModel.Detalles.Add(new NotaCreditoDetalleCreateViewModel
                            {
                                FacturaDetalleOriginalId = detalle.Id,
                                ProductoId = detalle.ProductoId,
                                ProductoNombre = detalle.ProductoNombre,
                                CantidadOriginal = detalle.Cantidad,
                                CantidadAcreditar = detalle.Cantidad,
                                PrecioUnitario = detalle.PrecioUnitario,
                                Subtotal = detalle.Subtotal,
                                PorcentajeImpuesto = detalle.PorcentajeImpuesto,
                                MontoImpuesto = detalle.MontoImpuesto,
                                DescuentoAplicado = detalle.DescuentoAplicado,
                                TotalLinea = detalle.TotalLinea,
                                Seleccionado = true
                            });
                        }
                    }
                    else
                    {
                        // Si ya hay detalles, solo actualizar la información del cliente
                        // (los detalles ya están en el viewModel desde el POST)
                        Console.WriteLine($"Detalles preservados: {viewModel.Detalles.Count} productos");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar datos de factura: {ex.Message}");
            }
        }
    }
}