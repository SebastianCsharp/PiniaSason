using Microsoft.AspNetCore.Mvc;
using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> ObtenerTodas()
    {
        var categorias = await _categoriaService.ObtenerTodasAsync();
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Crear(CrearCategoriaDto crearCategoriaDto)
    {
        var categoria = await _categoriaService.CrearAsync(crearCategoriaDto);
        return CreatedAtAction(nameof(ObtenerTodas), categoria);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        try
        {
            await _categoriaService.EliminarAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("mas-usadas")]
    public async Task<IActionResult> GetCategoriasMasUsadas()
    {
        var categorias = await _categoriaService.ObtenerCategoriasMasUsadasAsync();
        return Ok(categorias);
    }
}