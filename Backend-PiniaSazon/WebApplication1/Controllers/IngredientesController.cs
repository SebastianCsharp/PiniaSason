using Microsoft.AspNetCore.Mvc;
using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;

namespace PiniaSazon.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientesController : ControllerBase
{
    private readonly IIngredienteService _ingredienteService;

    public IngredientesController(IIngredienteService ingredienteService)
    {
        _ingredienteService = ingredienteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredienteDto>>> ObtenerTodos()
    {
        var ingredientes = await _ingredienteService.ObtenerTodosAsync();
        return Ok(ingredientes);
    }

}