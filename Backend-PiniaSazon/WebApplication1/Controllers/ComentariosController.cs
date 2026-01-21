using Microsoft.AspNetCore.Mvc;
using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;

namespace PiniaSazon.Api.Controllers;
[ApiController]
[Route("api/recetas/{recetaId}/[controller]")]
public class ComentariosController : ControllerBase
{
    private readonly IComentarioService _comentarioService;

    public ComentariosController(IComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ComentarioDto>>> ObtenerPorReceta(int recetaId)
    {
        var comentarios = await _comentarioService.ObtenerPorRecetaAsync(recetaId);
        return Ok(comentarios);
    }

    [HttpPost]
    public async Task<ActionResult<ComentarioDto>> Crear(int recetaId, CrearComentarioDto crearComentarioDto)
    {
        crearComentarioDto.RecetaId = recetaId;
        var comentario = await _comentarioService.CrearAsync(crearComentarioDto);
        return CreatedAtAction(nameof(ObtenerPorReceta), new { recetaId }, comentario);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ComentarioDto>> ActualizarComentario(int recetaId, int id, ActualizarComentarioDto actualizarDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comentarioActualizado = await _comentarioService.ActualizarComentarioAsync(recetaId, id, actualizarDto);

        if (comentarioActualizado == null)
            return NotFound(new { mensaje = "Comentario no encontrado" });

        return Ok(comentarioActualizado);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> EliminarComentario(int recetaId, int id)
    {
        var eliminado = await _comentarioService.EliminarComentarioAsync(recetaId, id);

        if (!eliminado)
            return NotFound(new { mensaje = "Comentario no encontrado" });

        return NoContent();
    }
}