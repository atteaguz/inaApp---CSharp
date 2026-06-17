using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Categoria;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        //obtener todas las categorias
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

        //obtener categoria por id
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _categoriaService.ObtenerPorIdsAsync(id);
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

        //crear nueva categoria
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CategoriaCreateDTO categoriaDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var response = await _categoriaService.CrearAsync(categoriaDTO);
                return Created("Categoria creada", response);
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        //actualizar categoria axistente
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> EditAsync(int id, [FromBody] CategoriaUpdateDTO categoriaDTO)
        {
            try
            {
                if (id != categoriaDTO.Id)
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var response = await _categoriaService.ActualizarAsync(categoriaDTO);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        //borrado logico de categoria existente
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
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }
    }
}