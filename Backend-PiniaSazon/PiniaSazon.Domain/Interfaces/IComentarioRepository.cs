using PiniaSazon.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Interfaces;

public interface IComentarioRepository
{
    Task<List<Comentario>> ObtenerPorRecetaAsync(int recetaId);
    Task<Comentario> CrearAsync(Comentario comentario);
    Task<Comentario?> ObtenerPorIdAsync(int id);
    Task EliminarAsync(int id);
    Task ActualizarAsync(Comentario comentario);
}