using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Cliente;
using InaApp.ProyectoINAApp.Models.Cliente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static inaApp.Common.Enums.Enumeradores;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ClienteController : Controller
    {

        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(
            IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService,
            IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            try
            {
                //obtener todos los clientes
                var lista = await _clienteService.ObtenerTodosAsync();

                var listaViewModel = _mapper.Map<List<ClienteIndexViewModel>>(lista.Data);

                //lista clientes se pasa a la vista por el model
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                ViewBag.Message = "No hay clientes disponibles.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al cargar la página: " + ex.Message;
                return View(new List<ClienteIndexViewModel>());
            }
        }

        // GET: ClienteController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdsAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var clienteVM = _mapper.Map<ClienteIndexViewModel>(response.Data);

                return View(clienteVM);
            }
            catch (NotFoundException)
            {
                TempData["Error"] = $"Cliente con ID {id} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al obtener los detalles del cliente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            var viewModel = new ClienteCreateViewModel();
            CargarTiposIdentificacion(viewModel);
            return View(viewModel);
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                CargarTiposIdentificacion(viewModel);
                return View(viewModel);
            }

            try
            {
                var clienteDTO = _mapper.Map<ClienteCreateDTO>(viewModel);
                var response = await _clienteService.CrearAsync(clienteDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    CargarTiposIdentificacion(viewModel);
                    return View(viewModel);
                }

                TempData["Mensaje"] = "Cliente creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el cliente: {ex.Message}");
                CargarTiposIdentificacion(viewModel);
                return View(viewModel);
            }
        }

        // GET: ClienteController/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdsAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var clienteVM = _mapper.Map<ClienteEditViewModel>(response.Data);
                CargarTiposIdentificacion(clienteVM, clienteVM.TipoIdentificacion);

                return View(clienteVM);
            }
            catch (NotFoundException)
            {
                TempData["Error"] = $"Cliente con ID {id} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al cargar el cliente para editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ClienteEditViewModel clienteEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarTiposIdentificacion(clienteEditVM, clienteEditVM.TipoIdentificacion);
                    return View(clienteEditVM);
                }

                var clienteDTO = _mapper.Map<ClienteUpdateDTO>(clienteEditVM);
                var response = await _clienteService.ActualizarAsync(clienteDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    CargarTiposIdentificacion(clienteEditVM, clienteEditVM.TipoIdentificacion);
                    return View(clienteEditVM);
                }

                TempData["Mensaje"] = "Cliente actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el cliente: {ex.Message}");
                CargarTiposIdentificacion(clienteEditVM, clienteEditVM.TipoIdentificacion);
                return View(clienteEditVM);
            }
        }

        // GET: ClienteController/Delete/5
        [HttpGet]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdsAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                var clienteVM = _mapper.Map<ClienteIndexViewModel>(response.Data);
                return View(clienteVM);
            }
            catch (NotFoundException)
            {
                TempData["Error"] = $"Cliente con ID {id} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al cargar el cliente para eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _clienteService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["Error"] = response.Message;
                }
                else
                {
                    TempData["Mensaje"] = "Cliente eliminado exitosamente.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al eliminar el cliente.";
                return RedirectToAction(nameof(Index));
            }
        }

        //metodos agregados para tipo identificacion
        private void CargarTiposIdentificacion(ClienteCreateViewModel viewModel, int? valorSeleccionado = null)
        {
            var tipos = Enum.GetValues(typeof(TipoIdentificacionEnum))
                .Cast<TipoIdentificacionEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = ObtenerNombreTipoIdentificacion(e),
                    Selected = valorSeleccionado.HasValue && (int)e == valorSeleccionado.Value
                })
                .ToList();

            viewModel.TiposIdentificacionList = tipos;
        }

        private void CargarTiposIdentificacion(ClienteEditViewModel viewModel, int? valorSeleccionado = null)
        {
            var tipos = Enum.GetValues(typeof(TipoIdentificacionEnum))
                .Cast<TipoIdentificacionEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = ObtenerNombreTipoIdentificacion(e),
                    Selected = valorSeleccionado.HasValue && (int)e == valorSeleccionado.Value
                })
                .ToList();

            viewModel.TiposIdentificacionList = tipos;
        }

        private string ObtenerNombreTipoIdentificacion(TipoIdentificacionEnum tipo)
        {
            return tipo switch
            {
                TipoIdentificacionEnum.CedulaFisica => "Cédula Física",
                TipoIdentificacionEnum.CedulaJuridica => "Cédula Jurídica",
                TipoIdentificacionEnum.DIMEX => "DIMEX",
                TipoIdentificacionEnum.Pasaporte => "Pasaporte",
                TipoIdentificacionEnum.NITE => "NITE",
                _ => tipo.ToString()
            };
        }
    }
}
