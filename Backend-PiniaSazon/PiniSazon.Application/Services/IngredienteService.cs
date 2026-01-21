using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;

namespace PiniaSazon.Application.Services;

public class IngredienteService : IIngredienteService
{
    private readonly IIngredienteRepository _ingredienteRepository;

    public IngredienteService(IIngredienteRepository ingredienteRepository)
    {
        _ingredienteRepository = ingredienteRepository;
    }

    public async Task<List<IngredienteDto>> ObtenerTodosAsync()
    {
        var ingredientes = await _ingredienteRepository.ObtenerTodosAsync();

        return ingredientes.Select(ingrediente => new IngredienteDto
        {
            Id = ingrediente.Id,
            Nombre = ingrediente.Nombre
        }).ToList();
    }
}