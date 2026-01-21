using Dapper;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiniaSazon.Infrastructure.Repositories;

public class ComentarioRepository : IComentarioRepository
{
    private readonly DatabaseContext _context;

    public ComentarioRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Comentario>> ObtenerPorRecetaAsync(int recetaId)
    { using var connection = _context.CreateConnection();
        var sql = @"
        SELECT 
            id as Id,
            receta_id as RecetaId,
            nombre_usuario as NombreUsuario,
            texto as Texto,
            calificacion as Calificacion,
            fecha_creacion as FechaCreacion
        FROM comentarios 
        WHERE receta_id = @RecetaId 
        ORDER BY fecha_creacion DESC";

        var comentarios = await connection.QueryAsync<Comentario>(sql, new { RecetaId = recetaId });
        return comentarios.ToList();
    }

    public async Task<Comentario> CrearAsync(Comentario comentario)
    {using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO comentarios (nombre_usuario, texto, calificacion, receta_id, fecha_creacion) 
                VALUES (@NombreUsuario, @Texto, @Calificacion, @RecetaId, @FechaCreacion) 
                RETURNING id";

        var id = await connection.ExecuteScalarAsync<int>(sql, new
        {
            comentario.NombreUsuario,
            comentario.Texto,
            comentario.Calificacion,
            comentario.RecetaId,
            FechaCreacion = DateTime.Now
        });

        comentario.Id = id;
        return comentario;
    }
    public async Task EliminarAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var sql = "DELETE FROM comentarios WHERE id = @Id";

        await connection.ExecuteAsync(sql, new { Id = id });
    }
    public async Task<Comentario?> ObtenerPorIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var sql = @"SELECT 
                    id,
                    texto,
                    calificacion,
                    receta_id as RecetaId,
                    nombre_usuario as NombreUsuario,
                    fecha_creacion as FechaCreacion
                FROM comentarios 
                WHERE id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Comentario>(sql, new { Id = id });
    }
    public async Task ActualizarAsync(Comentario comentario)
    {
        using var connection = _context.CreateConnection();

        var sql = @"UPDATE comentarios 
                SET texto = @Texto, 
                    calificacion = @Calificacion
                WHERE id = @Id";

        await connection.ExecuteAsync(sql, comentario);
    }
}