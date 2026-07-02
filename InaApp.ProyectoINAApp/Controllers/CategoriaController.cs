using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.Services;
using InaApp.ProyectoINAApp.Models.Categoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class CategoriaController : Controller
    {

        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> categoriaServ, IMapper mapper)
        {
            _categoriaService = categoriaServ;
            _mapper = mapper;
        }

        // GET: CategoriaController
        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                //obtener todos los productosL
                var lista = await _categoriaService.ObtenerTodosAsync();

                var listaViewModel = _mapper.Map<List<CategoriaIndexViewModel>>(lista.Data);

                //lista productos se pasa a la vista por el model
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                ViewBag.Message = "No hay Categorías disponibles.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "[ERROR] al cargar la pagina";
                return View();
            }
        }

        // GET: CategoriaController/Details/5
        [HttpGet]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var response = await _categoriaService.ObtenerPorIdsAsync(id);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = _mapper.Map<CategoriaIndexViewModel>(response.Data);
            return View(categoriaVM);
        }

        // GET: CategoriaController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CategoriaCreateViewModel categoriaVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(categoriaVM);
                }

                //mappear el viewmodel a DTO
                var categoriaDTO = _mapper.Map<CategoriaCreateDTO>(categoriaVM);

                //llamar al servicio para crear el producto
                var response = await _categoriaService.CrearAsync(categoriaDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    return View(categoriaVM);
                }

                TempData["Mensaje"] = "Categoría creada exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            var response = await _categoriaService.ObtenerPorIdsAsync(id);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = _mapper.Map<CategoriaEditViewModel>(response.Data);
            return View(categoriaVM);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, CategoriaEditViewModel categoriaVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(categoriaVM);
                }

                var categoriaDTO = _mapper.Map<CategoriaUpdateDTO>(categoriaVM);
                var response = await _categoriaService.ActualizarAsync(categoriaDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    return View(categoriaVM);
                }

                TempData["Mensaje"] = "Categoría actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Delete/5
        [HttpGet]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var response = await _categoriaService.ObtenerPorIdsAsync(id);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var categoriaVM = _mapper.Map<CategoriaIndexViewModel>(response.Data);
            return View(categoriaVM);
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, IFormCollection collection)
        {
            try
            {
                var response = await _categoriaService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                }
                else
                {
                    TempData["Mensaje"] = "Categoría eliminada exitosamente.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
