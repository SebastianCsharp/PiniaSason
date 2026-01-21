using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Services;

public class ComentarioService : IComentarioService
{
    private readonly IComentarioRepository _comentarioRepository;

    public ComentarioService(IComentarioRepository comentarioRepository)
    {
        _comentarioRepository = comentarioRepository;
    }
    public async Task<List<ComentarioDto>> ObtenerPorRecetaAsync(int recetaId)
    {
        var comentarios = await _comentarioRepository.ObtenerPorRecetaAsync(recetaId);

        return comentarios.Select(c => new ComentarioDto
        {
            Id = c.Id,
            NombreUsuario = c.NombreUsuario,
            Texto = c.Texto,
            Calificacion = c.Calificacion,
            FechaCreacion = c.FechaCreacion
        }).ToList();
    }

    public async Task<ComentarioDto> CrearAsync(CrearComentarioDto crearComentarioDto)
    {
        var comentario = new Comentario
        {
            Texto = crearComentarioDto.Texto,
            Calificacion = crearComentarioDto.Calificacion,
            RecetaId = crearComentarioDto.RecetaId,
            NombreUsuario = crearComentarioDto.NombreUsuario
        };

        var comentarioCreado = await _comentarioRepository.CrearAsync(comentario);

        return new ComentarioDto
        {
            Id = comentarioCreado.Id,
            NombreUsuario = comentarioCreado.NombreUsuario,
            Texto = comentarioCreado.Texto,
            Calificacion = comentarioCreado.Calificacion,
            FechaCreacion = comentarioCreado.FechaCreacion
        };
    }
    public async Task<bool> EliminarComentarioAsync(int recetaId, int comentarioId)
    {
        var comentario = await _comentarioRepository.ObtenerPorIdAsync(comentarioId);
        if (comentario == null || comentario.RecetaId != recetaId)
            return false;

        await _comentarioRepository.EliminarAsync(comentarioId);
        return true;
    }
    public async Task<ComentarioDto?> ActualizarComentarioAsync(int recetaId, int comentarioId, ActualizarComentarioDto actualizarDto)
    {
        var comentario = await _comentarioRepository.ObtenerPorIdAsync(comentarioId);

        if (comentario == null || comentario.RecetaId != recetaId)
            return null;
        comentario.Texto = actualizarDto.Texto;
        comentario.Calificacion = actualizarDto.Calificacion;

        await _comentarioRepository.ActualizarAsync(comentario);

        return new ComentarioDto
        {
            Id = comentario.Id,
            NombreUsuario = comentario.NombreUsuario,
            Texto = comentario.Texto,
            Calificacion = comentario.Calificacion,
            FechaCreacion = comentario.FechaCreacion
        };
    }
}