using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
