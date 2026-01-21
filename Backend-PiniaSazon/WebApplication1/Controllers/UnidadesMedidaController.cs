using Microsoft.AspNetCore.Mvc;
using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiniaSazon.Api.Controllers;

//  peticiones HTTP para unidades de medida
[ApiController] 
[Route("api/[controller]")] 
public class UnidadesMedidaController : ControllerBase
{
    private readonly IUnidadMedidaService _unidadMedidaService;
    public UnidadesMedidaController(IUnidadMedidaService unidadMedidaService)
    {
        _unidadMedidaService = unidadMedidaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UnidadMedidaDto>>> ObtenerTodas()
    {
        var unidades = await _unidadMedidaService.ObtenerTodasAsync();
        return Ok(unidades);
    }
}