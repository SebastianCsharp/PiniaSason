using Dapper;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;

namespace PiniaSazon.Infrastructure.Repositories;

public class IngredienteRepository : IIngredienteRepository
{
    private readonly DatabaseContext _context;

    public IngredienteRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Ingrediente>> ObtenerTodosAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT id as Id, nombre as Nombre FROM ingredientes ORDER BY nombre";
        var ingredientes = await connection.QueryAsync<Ingrediente>(sql);
        return ingredientes.ToList();
    }

    public async Task<Ingrediente?> ObtenerPorIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT id as Id, nombre as Nombre FROM ingredientes WHERE id = @Id";
        var ingrediente = await connection.QueryFirstOrDefaultAsync<Ingrediente>(sql, new { Id = id });
        return ingrediente;
    }

    public async Task<Ingrediente> CrearAsync(Ingrediente ingrediente)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO ingredientes (nombre) VALUES (@Nombre) RETURNING id";
        var id = await connection.ExecuteScalarAsync<int>(sql, ingrediente);
        ingrediente.Id = id;
        return ingrediente;
    }
}