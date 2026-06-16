using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Cliente;
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
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;

        public ClienteController(IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteServ)
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
        public async Task<ActionResult> Create([FromBody] ClienteCreateDTO clienteDTO)
        {
            try
            {
                var result = await _clienteService.CrearAsync(clienteDTO);
                return Created("Cliente creado: ", result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = "Datos inválidos", message = ex.Message });
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(new { error = "Campo requerido faltante", message = ex.Message });
            }
            catch (InvalidEmailFormatException ex)
            {
                return BadRequest(new { error = "Correo electrónico inválido", message = ex.Message });
            }
            catch (InvalidPhoneFormatException ex)
            {
                return BadRequest(new { error = "Teléfono inválido", message = ex.Message });
            }
            catch (InvalidIdentificationException ex)
            {
                return BadRequest(new { error = "Tipo de identificación inválido", message = ex.Message });
            }
            catch (DuplicateIdentificationException ex)
            {
                return BadRequest(new { error = "Identificación duplicada", message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear el cliente");
            }
        }

        //modificar cliente existente
        [HttpPatch("update/{IdCliente}")]
        public async Task<ActionResult> EditAsync(int IdCliente, [FromBody] ClienteUpdateDTO clienteDTO)
        {
            try
            {
                if (IdCliente != clienteDTO.IdCliente)
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del cuerpo" });

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _clienteService.ActualizarAsync(clienteDTO);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Argumento inválido", message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = "Cliente no encontrado", message = ex.Message });
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(new { error = "Campo requerido faltante", message = ex.Message });
            }
            catch (InvalidEmailFormatException ex)
            {
                return BadRequest(new { error = "Correo electrónico inválido", message = ex.Message });
            }
            catch (InvalidPhoneFormatException ex)
            {
                return BadRequest(new { error = "Teléfono inválido", message = ex.Message });
            }
            catch (DuplicateIdentificationException ex)
            {
                return BadRequest(new { error = "Identificación duplicada", message = ex.Message });
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
