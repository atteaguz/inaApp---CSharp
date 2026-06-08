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
                    return NotFound("No hay datos disponibles");
                }

                return Ok(lista);
            }
            catch (Exception)
            {

                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        // GET: ProductoController/Details/5
        [HttpGet("getbyid/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            try
            {
                var result = await _productoService.CrearAsync(producto);
                return Created("Producto creado", result);
            }
            catch (Exception)
            {
                return BadRequest("Error al crear el producto");
            }
        }

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) { return BadRequest("Error al eliminar, id incorrecto");}
                
                var result = await _productoService.EliminarAsync(id);
                return result ? Ok("Producto eliminado") : BadRequest("Error al eliminar el producto");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }
    }
}
