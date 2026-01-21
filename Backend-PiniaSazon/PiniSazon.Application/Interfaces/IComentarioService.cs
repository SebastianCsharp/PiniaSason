using PiniaSazon.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Interfaces;

public interface IComentarioService
{
    Task<List<ComentarioDto>> ObtenerPorRecetaAsync(int recetaId);
    Task<ComentarioDto> CrearAsync(CrearComentarioDto crearComentarioDto);
    Task<bool> EliminarComentarioAsync(int recetaId, int comentarioId);
    Task<ComentarioDto?> ActualizarComentarioAsync(int recetaId, int comentarioId, ActualizarComentarioDto actualizarDto);
}