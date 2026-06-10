using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {
        private readonly IGenericService<Producto> _productoService;

        //inyectar el servicio en el controlador
        public ProductoController(IGenericService<Producto> productoServ)
        {
            _productoService = productoServ;
        }

        //obtener todos los productos
        [HttpGet("getall")]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var lista = await _productoService.ObtenerTodosAsync();

                if (lista.Count == 0) {
                    return NotFound("No hay productos disponibles");
                }

                return Ok(lista);
            }
            catch (Exception)
            {

                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        //obtener un producto por id
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdsAsync(id);
                return Ok(producto);
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

        //crear un nuevo producto
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            try
            {
                var result = await _productoService.CrearAsync(producto);
                return Created("Producto creado", result);
            }
            catch (InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedNameException ex)
            {
                return BadRequest(ex.Message);
            }
            
            catch (Exception ex)
            {
                return BadRequest("Error al crear el producto");
            }
        }

        //modificar un producto existente
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> EditAsync(int id, [FromBody] Producto producto)
        {
            try
            {
                var result = await _productoService.ActualizarAsync(producto);
                return Ok(result);
            }
            catch (InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el producto. Contacte al administrador");
            }
        }

        //eliminar un producto - borrado logico
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //if (id <= 0) { return BadRequest("Error al eliminar, id incorrecto");}
                
                var result = await _productoService.EliminarAsync(id);
                return Ok("Producto eliminado.");
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
