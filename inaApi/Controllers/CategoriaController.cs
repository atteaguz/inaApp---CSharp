using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : Controller
    {
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;

        public CategoriaController(IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> categoriaService)
        {
            _categoriaService = categoriaService;
        }

        //obtener todos los productos
        [HttpGet("getall")]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var response = await _categoriaService.ObtenerTodosAsync();
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

        //obtener una categoria por id
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            throw new NotImplementedException();
        }

        //crear una nueva categoria
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CategoriaCreateDTO categoriaDTO)
        {
            throw new NotImplementedException();
        }

        //modificar una categroia existente
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> EditAsync(int id, [FromBody] CategoriaUpdateDTO categoriaDTO)
        {
            throw new NotImplementedException();
        }

        //eliminar una categoria existente - borrado logico
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _categoriaService.EliminarAsync(id);
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
