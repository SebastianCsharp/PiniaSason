using Microsoft.AspNetCore.Mvc;
using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;

namespace PiniaSazon.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecetasController : ControllerBase
{
    private readonly IRecetaService _recetaService;

    public RecetasController(IRecetaService recetaService)
    {
        _recetaService = recetaService;
    }
    [HttpGet]
    public async Task<ActionResult<List<RecetaDto>>> ObtenerTodas()
    {
        var recetas = await _recetaService.ObtenerTodasRecetasAsync();
        return Ok(recetas);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<RecetaDto>> ObtenerPorId(int id)
    {
        var receta = await _recetaService.ObtenerRecetaPorIdAsync(id);

        if (receta == null)
            return NotFound(new { mensaje = "Receta no encontrada" });

        return Ok(receta);
    }

    [HttpGet("categoria/{categoriaNombre}")]
    public async Task<ActionResult<List<RecetaDto>>> ObtenerPorCategoria(string categoriaNombre)
    {
        var recetas = await _recetaService.ObtenerRecetaPorCategoriaAsync(categoriaNombre);
        return Ok(recetas);
    }

    [HttpPost]
    public async Task<ActionResult<RecetaDto>> CrearReceta(CrearRecetaDto crearRecetaDto)
    {
        var receta = await _recetaService.CrearRecetaAsync(crearRecetaDto);
        return Ok(receta);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Eliminar(int id)
    {
        var eliminado = await _recetaService.EliminarRecetaAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "Receta no encontrada" });

        return NoContent();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<RecetaDto>> ActualizarReceta(int id, ActualizarRecetaDto actualizarRecetaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var recetaActualizada = await _recetaService.ActualizarRecetaAsync(id, actualizarRecetaDto);

        if (recetaActualizada == null)
            return NotFound(new { mensaje = "Receta no encontrada" });

        return Ok(recetaActualizada);
    }
    [HttpGet("destacadas")]
    public async Task<ActionResult<List<RecetaDto>>> GetRecetasDestacadas()
    {
        try
        {
            var recetasDestacadas = await _recetaService.ObtenerRecetasDestacadasAsync();
            return Ok(recetasDestacadas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener recetas destacadas: {ex.Message}");
        }
    }
}