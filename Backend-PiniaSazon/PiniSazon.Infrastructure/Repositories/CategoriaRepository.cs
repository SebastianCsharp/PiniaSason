using Dapper;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;


namespace PiniaSazon.Infrastructure.Repositories;

public class CategoriaRepository:ICategoriaRepository
{
    private readonly DatabaseContext _context;

    public CategoriaRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> ObtenerTodasAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM categorias ORDER BY nombre";
        var categorias = await connection.QueryAsync<Categoria>(sql);
        return categorias.ToList();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM categorias WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Categoria>(sql, new { Id = id });
    }

    public async Task<Categoria> CrearAsync(Categoria categoria)
    {
        using var connection = _context.CreateConnection();
        var sql = "INSERT INTO categorias (nombre) VALUES (@Nombre) RETURNING id";
        var id = await connection.ExecuteScalarAsync<int>(sql, categoria);
        categoria.Id = id;
        return categoria;
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        using var connection = _context.CreateConnection();
        var sql = "UPDATE categorias SET nombre = @Nombre WHERE id = @Id";
        await connection.ExecuteAsync(sql, categoria);
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = "DELETE FROM categorias WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
    public async Task<List<Categoria>> ObtenerCategoriasConConteoRecetasAsync()
    {
        using var connection = _context.CreateConnection();

        var sql = @"
    SELECT 
        c.id,
        c.nombre,
        c.cantidad_recetas as CantidadRecetas  
    FROM categorias c
    ORDER BY c.cantidad_recetas DESC"; 

        var resultados = await connection.QueryAsync<Categoria>(sql);
        return resultados.ToList();
    }
}
