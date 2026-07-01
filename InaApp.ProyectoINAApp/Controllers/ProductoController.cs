using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Producto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IMapper _mapper;

        public ProductoController(IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoServ, IMapper mapper)
        {
            _productoService = productoServ;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductoController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
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
                    return View(productoVM);
                }

                //mappear el viewmodel a DTO
                var productoDTO = _mapper.Map<ProductoCreateDTO>(productoVM);

                //llamar al servicio para crear el producto
                var response = await _productoService.CrearAsync(productoDTO);

                if (!response.Success) {
                    ModelState.AddModelError("", response.Message);
                    return View(productoVM);
                }

                TempData["Mensaje"] = "Producto creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                    return View(productoEditVM);
                }

                var productoDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);

                var response = await _productoService.ActualizarAsync(productoDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    return View(productoEditVM);
                }

                TempData["Mensaje"] = "Producto modificado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
