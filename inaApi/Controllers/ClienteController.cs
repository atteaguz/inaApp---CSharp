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
        [HttpGet("getbyid/{IdCliente}")]
        public async Task<ActionResult> Details(int IdCliente)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdsAsync(IdCliente);
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
        public async Task<ActionResult> Create([FromBody] Cliente cliente)
        {
            try
            {
                var result = await _clienteService.CrearAsync(cliente);
                return Created("Cliente creado: ", result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear el cliente");
            }
        }

        //modificar cliente existente
        [HttpPatch("update/{IdCliente}")]
        public async Task<ActionResult> EditAsync(int IdCliente, [FromBody] Cliente cliente)
        {
            try
            {
                var result = await _clienteService.ActualizarAsync(cliente);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el cliente. Contacte al administrador");
            }
        }

        //eliminar cliente existente - borrado logico
        [HttpDelete("delete/{IdCliente}")]
        public async Task<ActionResult> Delete(int IdCliente)
        {
            try
            {
                var result = await _clienteService.EliminarAsync(IdCliente);
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
