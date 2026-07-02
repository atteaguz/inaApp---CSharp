using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Categoria;
using InaApp.ProyectoINAApp.Models.Producto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public ProductoController(
            IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoServ,
            IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> categoriaServ,
            IMapper mapper)
        {
            _productoService = productoServ;
            _categoriaService = categoriaServ;
            _mapper = mapper;
        }


        // GET: ProductoController
        public async Task<ActionResult> Index()
        {
            try
            {
                //obtener todos los productosL
                var lista = await _productoService.ObtenerTodosAsync();

                var listaViewModel = _mapper.Map<List<ProductoIndexViewModel>>(lista.Data);

                //lista productos se pasa a la vista por el model
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                ViewBag.Message = "No hay Productos disponibles.";
                return View();
            }
            catch (Exception ex){
                ViewBag.Message = "[ERROR] al cargar la pagina";
                return View();
            }
        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var response = await _productoService.ObtenerPorIdsAsync(id);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var productoVM = _mapper.Map<ProductoIndexViewModel>(response.Data);
            return View(productoVM);
        }

        // GET: ProductoController/Create
        [HttpGet]
        public async Task<ActionResult> CreateAsync()
        {
            var viewModel = new ProductoCreateViewModel();
            await CargarCategoriasDropDown(viewModel);
            return View(viewModel);
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ProductoCreateViewModel productoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCategoriasDropDown(productoVM);
                    return View(productoVM);
                }

                //mappear el viewmodel a DTO
                var productoDTO = _mapper.Map<ProductoCreateDTO>(productoVM);

                //llamar al servicio para crear el producto
                var response = await _productoService.CrearAsync(productoDTO);

                if (!response.Success) {
                    ModelState.AddModelError("", response.Message);
                    await CargarCategoriasDropDown(productoVM);
                    return View(productoVM);
                }

                TempData["Mensaje"] = "Producto creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await CargarCategoriasDropDown(productoVM);
                return View(productoVM);
            }
        }

        // GET: ProductoController/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            var response = await _productoService.ObtenerPorIdsAsync(id);

            if (!response.Success) { 
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var productoVM = _mapper.Map<ProductoEditViewModel>(response.Data);
            await CargarCategoriasDropDown(productoVM, productoVM.CategoriaId);

            return View(productoVM);
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ProductoEditViewModel productoEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCategoriasDropDown(productoEditVM, productoEditVM.CategoriaId);
                    return View(productoEditVM);
                }

                var productoDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);

                var response = await _productoService.ActualizarAsync(productoDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    await CargarCategoriasDropDown(productoEditVM, productoEditVM.CategoriaId);
                    return View(productoEditVM);
                }

                TempData["Mensaje"] = "Producto modificado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await CargarCategoriasDropDown(productoEditVM, productoEditVM.CategoriaId);
                return View(productoEditVM);
            }
        }

        // GET: ProductoController/Delete/5
        [HttpGet]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var response = await _productoService.ObtenerPorIdsAsync(id);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var productoVM = _mapper.Map<ProductoIndexViewModel>(response.Data);
            return View(productoVM);
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _productoService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Mensaje"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //metodos agregados para cargar el dropdown de categorias en las vistas de crear y editar productos
        private async Task CargarCategoriasDropDown(ProductoCreateViewModel viewModel, int? categoriaSeleccionada = null)
        {
            try
            {
                var categorias = await _categoriaService.ObtenerTodosAsync();

                viewModel.CategoriasList = categorias.Data
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre,
                        Selected = categoriaSeleccionada.HasValue && c.Id == categoriaSeleccionada.Value
                    })
                    .ToList();
            }
            catch
            {
                viewModel.CategoriasList = new List<SelectListItem>();
            }
        }

        private async Task CargarCategoriasDropDown(ProductoEditViewModel viewModel, int? categoriaSeleccionada = null)
        {
            try
            {
                var categorias = await _categoriaService.ObtenerTodosAsync();

                ViewBag.Categorias = categorias.Data
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre,
                        Selected = categoriaSeleccionada.HasValue && c.Id == categoriaSeleccionada.Value
                    })
                    .ToList();
            }
            catch
            {
                ViewBag.Categorias = new List<SelectListItem>();
            }
        }
    }
}
