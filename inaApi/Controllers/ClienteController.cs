using inaApp.Common.Exceptions;
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
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdsAsync(id);
                return Ok(cliente);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        //crear nuevo cliente
        [HttpPost("create")]
        public ActionResult Create()
        {
            return View();
        }

        //modificar cliente existente
        [HttpPatch("edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        //eliminar cliente existente - borrado logico
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _clienteService.EliminarAsync(id);
                return Ok("Cliente eliminado.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }
    }
}
