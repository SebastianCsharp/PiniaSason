using PiniaSazon.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PiniaSazon.Application.Interfaces;

public interface IRecetaService
{
    Task<List<RecetaDto>> ObtenerTodasRecetasAsync();
    Task<List<RecetaDto>> ObtenerRecetaPorCategoriaAsync(string categoriaNombre);
    Task<RecetaDto> CrearRecetaAsync(CrearRecetaDto crearRecetaDto);
    Task<bool> EliminarRecetaAsync(int id);
    Task<RecetaDto?> ActualizarRecetaAsync(int id, ActualizarRecetaDto actualizarRecetaDto);
    Task<RecetaDto?> ObtenerRecetaPorIdAsync(int id);
    Task<List<RecetaDto>> ObtenerRecetasDestacadasAsync();
}