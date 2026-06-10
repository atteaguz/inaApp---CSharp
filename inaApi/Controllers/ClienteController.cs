using inaApp.Common.interfaces;
using inaApp.Entities;
using inaApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {

        //inyeccion de ClienteService
        private readonly IGenericService<Cliente> _clienteService;

        public ClienteController(IGenericService<Cliente> clienteServ)
        {
            _clienteService = clienteServ;
        }

        //obtener todos los clientes
        [HttpGet("getall")]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var lista = await _clienteService.ObtenerTodosAsync();

                if (lista.Count == 0)
                {
                    return NotFound("No hay clientes disponibles");
                }
                return Ok(lista);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        //obtener cliente por id
        [HttpGet("getbyid/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        //crear nuevo cliente
        public ActionResult Create()
        {
            return View();
        }

        //modificar cliente existente
        public ActionResult Edit(int id)
        {
            return View();
        }

        //eliminar cliente existente - borrado logico
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
