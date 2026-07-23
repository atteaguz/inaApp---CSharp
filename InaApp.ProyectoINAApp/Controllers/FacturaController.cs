using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.DTOs.Factura;
using inaApp.Services;
using InaApp.ProyectoINAApp.Models.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly FacturaService _facturaService;
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;
        private readonly IMapper _mapper;

        private const decimal PORCENTAJE_DESCUENTO = 0.05m;

        public FacturaController(
            FacturaService facturaService,
            ClienteService clienteService,
            ProductoService productoService,
            IMapper mapper)
        {
            _facturaService = facturaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _mapper = mapper;
        }

        // GET: FacturaController/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await _facturaService.ObtenerTodosAsync();

                if (!response.Success || response.Data == null || !response.Data.Any())
                {
                    ViewBag.Message = "No hay facturas registradas.";
                    return View(new List<FacturaIndexViewModel>());
                }

                var listaViewModel = _mapper.Map<List<FacturaIndexViewModel>>(response.Data);
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                ViewBag.Message = "No hay facturas registradas.";
                return View(new List<FacturaIndexViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al cargar la página: " + ex.Message;
                return View(new List<FacturaIndexViewModel>());
            }
        }

        // GET: FacturaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdsAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = _mapper.Map<FacturaDetailsViewModel>(response.Data);
                return View(viewModel);
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al obtener los detalles de la factura.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: FacturaController/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new FacturaCreateViewModel();
            await CargarClientes(viewModel);
            await CargarProductos(viewModel);
            return View(viewModel);
        }

        // POST: FacturaController/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(FacturaCreateViewModel viewModel)
        {
            try
            {
                //validar producto seleccionado
                var productoResponse = await _productoService.ObtenerPorIdsAsync(viewModel.ProductoId);
                if (!productoResponse.Success)
                {
                    viewModel.Error = "Producto no encontrado";
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View("Create", viewModel);
                }

                var producto = productoResponse.Data;

                //validar cantidad
                if (viewModel.Cantidad > producto.Stock)
                {
                    viewModel.Error = $"Stock insuficiente. Disponible: {producto.Stock}";
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View("Create", viewModel);
                }

                //validar que el producto no se agregue dos veces
                if (viewModel.Detalles.Any(d => d.ProductoId == viewModel.ProductoId))
                {
                    viewModel.Error = $"El producto '{producto.Nombre}' ya fue agregado";
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View("Create", viewModel);
                }

                //agregar producto al detalle
                var detalle = new FacturaDetalleViewModel
                {
                    ProductoId = viewModel.ProductoId,
                    ProductoNombre = producto.Nombre,
                    Cantidad = viewModel.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = viewModel.Cantidad * producto.Precio,
                    StockDisponible = producto.Stock
                };

                viewModel.Detalles.Add(detalle);
                RecalcularTotales(viewModel);

                //limpiar campos de producto y cantidad para agregar otro producto
                viewModel.ProductoId = 0;
                viewModel.Cantidad = 1;
                viewModel.Error = null;

                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View("Create", viewModel);
            }
            catch (Exception ex)
            {
                viewModel.Error = $"Error al agregar producto: {ex.Message}";
                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View("Create", viewModel);
            }
        }

        // POST: FacturaController/RemoveProduct
        [HttpPost]
        public async Task<IActionResult> RemoveProduct(FacturaCreateViewModel viewModel, int productoId)
        {
            var detalle = viewModel.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle != null)
            {
                viewModel.Detalles.Remove(detalle);
                RecalcularTotales(viewModel);
            }

            viewModel.ProductoId = 0;
            viewModel.Cantidad = 1;
            viewModel.Error = null;

            await CargarClientes(viewModel);
            await CargarProductos(viewModel);
            return View("Create", viewModel);
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacturaCreateViewModel viewModel)
        {
            try
            {
                //validar cliente seleccionado
                if (viewModel.ClienteId <= 0)
                {
                    ModelState.AddModelError("ClienteId", "Debe seleccionar un cliente");
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View(viewModel);
                }

                //validar que haya al menos un producto en el detalle
                if (!viewModel.Detalles.Any())
                {
                    viewModel.Error = "Debe agregar al menos un producto";
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View(viewModel);
                }

                //convertir el viewModel a DTO para enviar al servicio
                var dto = new FacturaCreateDTO
                {
                    ClienteId = viewModel.ClienteId,
                    Fecha = DateTime.Now
                };

                foreach (var detalle in viewModel.Detalles)
                {
                    dto.Detalles.Add(new FacturaDetalleCreateDTO
                    {
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad
                    });
                }

                //crear la factura usando el servicio
                var response = await _facturaService.CrearAsync(dto);

                if (!response.Success)
                {
                    viewModel.Error = response.Message;
                    await CargarClientes(viewModel);
                    await CargarProductos(viewModel);
                    return View(viewModel);
                }

                TempData["Mensaje"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                viewModel.Error = ex.Message;
                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View(viewModel);
            }
            catch (InsufficientStockException ex)
            {
                viewModel.Error = ex.Message;
                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View(viewModel);
            }
            catch (ClienteInactivoException ex)
            {
                viewModel.Error = ex.Message;
                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                viewModel.Error = $"Error al crear la factura: {ex.Message}";
                await CargarClientes(viewModel);
                await CargarProductos(viewModel);
                return View(viewModel);
            }
        }

        // GET: FacturaController/Anular/5
        public async Task<ActionResult> Anular(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdsAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = _mapper.Map<FacturaDetailsViewModel>(response.Data);
                return View(viewModel);
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al cargar la factura para anular.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: FacturaController/Anular/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AnularConfirmado(int id)
        {
            try
            {
                var response = await _facturaService.AnularAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                }
                else
                {
                    TempData["Mensaje"] = response.Message;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al anular la factura: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        //metodos auxiliares para cargar clientes y productos
        private async Task CargarClientes(FacturaCreateViewModel viewModel)
        {
            try
            {
                var response = await _clienteService.ObtenerTodosAsync();
                if (response.Success && response.Data != null)
                {
                    viewModel.ClientesList = response.Data
                        .Select(c => new SelectListItem
                        {
                            Value = c.IdCliente.ToString(),
                            Text = $"{c.Nombre} {c.PrimerApellido} {c.SegundoApellido ?? ""}".Trim(),
                            Selected = c.IdCliente == viewModel.ClienteId
                        })
                        .ToList();

                    // Agregar opción por defecto
                    viewModel.ClientesList.Insert(0, new SelectListItem
                    {
                        Value = "",
                        Text = ">Seleccione un cliente"
                    });
                }
            }
            catch (Exception)
            {
                viewModel.ClientesList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = ">No hay clientes disponibles" }
                };
            }
        }

        private async Task CargarProductos(FacturaCreateViewModel viewModel)
        {
            try
            {
                var response = await _productoService.ObtenerTodosAsync();
                if (response.Success && response.Data != null)
                {
                    // Filtrar productos con stock > 0 y activos
                    var productosDisponibles = response.Data
                        .Where(p => p.Estado && p.Stock > 0)
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.Nombre} - Stock: {p.Stock} - ₡{p.Precio:N2}",
                            Selected = p.Id == viewModel.ProductoId
                        })
                        .ToList();

                    // Remover productos ya agregados al detalle
                    var productosIdsAgregados = viewModel.Detalles.Select(d => d.ProductoId).ToList();
                    productosDisponibles = productosDisponibles
                        .Where(p => !productosIdsAgregados.Contains(int.Parse(p.Value)))
                        .ToList();

                    viewModel.ProductosList = productosDisponibles;

                    viewModel.ProductosList.Insert(0, new SelectListItem
                    {
                        Value = "",
                        Text = ">Seleccione un producto"
                    });
                }
            }
            catch (Exception)
            {
                viewModel.ProductosList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = ">No hay productos disponibles" }
                };
            }
        }

        private void RecalcularTotales(FacturaCreateViewModel viewModel)
        {
            viewModel.Subtotal = viewModel.Detalles.Sum(d => d.Subtotal);
            viewModel.Descuento = Math.Round(viewModel.Subtotal * PORCENTAJE_DESCUENTO, 2);
            viewModel.Total = Math.Round(viewModel.Subtotal - viewModel.Descuento, 2);
        }
    }
}