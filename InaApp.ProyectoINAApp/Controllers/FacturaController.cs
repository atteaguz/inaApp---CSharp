using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
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
        private readonly CategoriaService _categoriaService;
        private readonly IMapper _mapper;

        private const decimal PORCENTAJE_DESCUENTO = 0.05m;

        public FacturaController(
            FacturaService facturaService,
            ClienteService clienteService,
            ProductoService productoService,
            CategoriaService categoriaService,
            IMapper mapper)
        {
            _facturaService = facturaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _categoriaService = categoriaService;
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
            await CargarDatos(viewModel);
            return View(viewModel);
        }

        // POST: FacturaController/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(FacturaCreateViewModel viewModel)
        {
            try
            {
                //validaciones para ux
                //no reemplazan las validaciones del service

                //validar que se haya seleccionado un producto
                if (viewModel.ProductoId <= 0)
                {
                    viewModel.Error = "Debe seleccionar un producto";
                    await CargarDatos(viewModel);
                    return View("Create", viewModel);
                }

                //validar que la cantidad sea mayor a 0
                if (viewModel.Cantidad <= 0)
                {
                    viewModel.Error = "La cantidad debe ser mayor a 0";
                    await CargarDatos(viewModel);
                    return View("Create", viewModel);
                }

                //obtener el producto solo para mostrarlo en la vista
                var productoResponse = await _productoService.ObtenerPorIdsAsync(viewModel.ProductoId);
                if (!productoResponse.Success)
                {
                    viewModel.Error = "Producto no encontrado";
                    await CargarDatos(viewModel);
                    return View("Create", viewModel);
                }

                var producto = productoResponse.Data;

                //validar el stock disponible
                if (viewModel.Cantidad > producto.Stock)
                {
                    viewModel.Error = $"Stock insuficiente. Disponible: {producto.Stock}";
                    await CargarDatos(viewModel);
                    return View("Create", viewModel);
                }

                //validar que no se agregue dos veces el mismo producto
                if (viewModel.Detalles.Any(d => d.ProductoId == viewModel.ProductoId))
                {
                    viewModel.Error = $"El producto '{producto.Nombre}' ya fue agregado";
                    await CargarDatos(viewModel);
                    return View("Create", viewModel);
                }

                //agregar el producto al detalle de la factura
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

                // Limpiar campos para agregar otro producto
                viewModel.ProductoId = 0;
                viewModel.Cantidad = 1;
                viewModel.Error = null;

                //cargar los datos a la vista
                await CargarDatos(viewModel);
                return View("Create", viewModel);
            }
            catch (Exception ex)
            {
                viewModel.Error = $"Error al agregar producto: {ex.Message}";
                await CargarDatos(viewModel);
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

            await CargarDatos(viewModel);
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
                    await CargarDatos(viewModel);
                    return View(viewModel);
                }

                //validar que haya al menos un producto en el detalle
                if (!viewModel.Detalles.Any())
                {
                    viewModel.Error = "Debe agregar al menos un producto";
                    await CargarDatos(viewModel);
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
                    await CargarDatos(viewModel);
                    return View(viewModel);
                }

                TempData["Mensaje"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                viewModel.Error = ex.Message;
                await CargarDatos(viewModel);
                return View(viewModel);
            }
            catch (InsufficientStockException ex)
            {
                viewModel.Error = ex.Message;
                await CargarDatos(viewModel);
                return View(viewModel);
            }
            catch (ClienteInactivoException ex)
            {
                viewModel.Error = ex.Message;
                await CargarDatos(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                viewModel.Error = $"Error al crear la factura: {ex.Message}";
                await CargarDatos(viewModel);
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

        // GET: FacturaController/BuscarClientes (Popup)
        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string termino = "", int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _clienteService.ObtenerTodosAsync();
                var clientes = response.Data ?? new List<ClienteResponseDTO>();

                //filtro
                if (!string.IsNullOrWhiteSpace(termino))
                {
                    clientes = clientes.Where(c =>
                        c.NumeroIdentificacion.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
                        $"{c.Nombre} {c.PrimerApellido} {c.SegundoApellido ?? ""}".Contains(termino, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                //paginacion
                var total = clientes.Count;
                var items = clientes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.Termino = termino;

                return PartialView("_ClientesPopup", items);
            }
            catch (Exception)
            {
                return PartialView("_ClientesPopup", new List<ClienteResponseDTO>());
            }
        }

        // GET: FacturaController/BuscarProductos (Popup)
        [HttpGet]
        public async Task<IActionResult> BuscarProductos(string termino = "", int categoriaId = 0, int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _productoService.ObtenerTodosAsync();
                var productos = response.Data ?? new List<ProductoResponseDTO>();

                //filtrar solo activos y con stock > 0
                productos = productos.Where(p => p.Estado && p.Stock > 0).ToList();

                //filtrar por termino
                if (!string.IsNullOrWhiteSpace(termino))
                {
                    productos = productos.Where(p =>
                        p.Id.ToString().Contains(termino) ||
                        p.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                //filtrar por categoria
                if (categoriaId > 0)
                {
                    productos = productos.Where(p => p.CategoriaId == categoriaId).ToList();
                }

                //paginacion
                var total = productos.Count;
                var items = productos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.Termino = termino;
                ViewBag.CategoriaId = categoriaId;

                //cargar categorias para el filtro
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                ViewBag.CategoriasList = categoriasResponse.Data?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == categoriaId
                }).ToList() ?? new List<SelectListItem>();

                return PartialView("_ProductosPopup", items);
            }
            catch (Exception)
            {
                return PartialView("_ProductosPopup", new List<ProductoResponseDTO>());
            }
        }

        // POST: FacturaController/SeleccionarCliente
        [HttpPost]
        public IActionResult SeleccionarCliente(int clienteId, string clienteNombre)
        {
            TempData["ClienteSeleccionadoId"] = clienteId;
            TempData["ClienteSeleccionadoNombre"] = clienteNombre;
            return Json(new { success = true });
        }

        // POST: FacturaController/SeleccionarProducto
        [HttpPost]
        public async Task<IActionResult> SeleccionarProducto(int productoId, int cantidad)
        {
            var productoResponse = await _productoService.ObtenerPorIdsAsync(productoId);
            if (!productoResponse.Success)
            {
                return Json(new { success = false, error = "Producto no encontrado" });
            }

            var producto = productoResponse.Data;

            return Json(new
            {
                success = true,
                productoId = producto.Id,
                productoNombre = producto.Nombre,
                precio = producto.Precio,
                stock = producto.Stock,
                tipoImpuesto = producto.TipoImpuesto.ToString(),
                porcentajeImpuesto = producto.PorcentajeImpuesto,
                descuentoMaximo = producto.DescuentoMaximo
            });
        }

        //metodos auxiliares para cargar clientes y productos
        private async Task CargarDatos(FacturaCreateViewModel viewModel)
        {
            await CargarClientes(viewModel);
            await CargarProductos(viewModel);
        }

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