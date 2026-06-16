using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;

        //inyectar el servicio en el controlador
        public ProductoController(IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoServ)
        {
            _productoService = productoServ;
        }

        //obtener todos los productos
        [HttpGet("getall")]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var response = await _productoService.ObtenerTodosAsync();
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult> Create([FromBody] ProductoCreateDTO productoDTO)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var response = await _productoService.CrearAsync(productoDTO);
                return Created("Producto creado", response);
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
        public async Task<ActionResult> EditAsync(int id, [FromBody] ProductoUpdateDTO productoDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var response = await _productoService.ActualizarAsync(productoDTO);
                return Ok(response);
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
                var response = await _productoService.EliminarAsync(id);
                return response.Data ? Ok(response) : BadRequest(response);
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
