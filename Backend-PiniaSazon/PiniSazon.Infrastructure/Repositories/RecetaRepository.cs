using Dapper;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;

namespace PiniaSazon.Infrastructure.Repositories;

public class RecetaRepository : IRecetaRepository
{
    private readonly DatabaseContext _context;

    public RecetaRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Receta>> ObtenerTodasAsync()
    {
        using var connection = _context.CreateConnection();

        var sqlRecetas = @"SELECT 
                    id as Id,
                    titulo as Titulo,
                    descripcion as Descripcion,
                    tiempo_preparacion as TiempoPreparacion,
                    pasos as Pasos,
                    categoria_id as CategoriaId
                    FROM recetas";

        var recetas = (await connection.QueryAsync<Receta>(sqlRecetas)).ToList();
        foreach (var receta in recetas)
        {
            if (receta.CategoriaId > 0)
            {
                var sqlCategoria = @"SELECT 
                            id as Id,
                            nombre as Nombre
                            FROM categorias 
                            WHERE id = @CategoriaId";

                var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(
                    sqlCategoria,
                    new { CategoriaId = receta.CategoriaId }
                );

                receta.Categoria = categoria;
            }
        }
        foreach (var receta in recetas)
        {
            var sqlIngredientes = @"SELECT 
                              ri.ingrediente_id,
                              ri.cantidad,
                              ri.unidad_medida_id,
                              um.nombre as unidad_nombre,
                              um.abreviatura as unidad_abreviatura,
                              i.nombre
                              FROM receta_ingredientes ri
                              INNER JOIN ingredientes i ON ri.ingrediente_id = i.id
                              INNER JOIN unidades_medida um ON ri.unidad_medida_id = um.id
                              WHERE ri.receta_id = @RecetaId";

            var ingredientesData = await connection.QueryAsync<dynamic>(sqlIngredientes, new { RecetaId = receta.Id });

            receta.RecetaIngredientes = ingredientesData.Select(item => new RecetaIngrediente
            {
                RecetaId = receta.Id,
                IngredienteId = (int)item.ingrediente_id,
                Cantidad = item.cantidad ?? 0m,
                UnidadMedidaId = (int)item.unidad_medida_id,
                UnidadMedida = new UnidadMedida
                {
                    Id = (int)item.unidad_medida_id,
                    Nombre = item.unidad_nombre ?? "",
                    Abreviatura = item.unidad_abreviatura ?? ""
                },
                Ingrediente = new Ingrediente
                {
                    Id = (int)item.ingrediente_id,
                    Nombre = item.nombre ?? ""
                }
            }).ToList();
        }

        return recetas;
    }
    public async Task<List<Receta>> ObtenerPorCategoriaAsync(string categoriaNombre)
    {using var connection = _context.CreateConnection();
        var sqlRecetas = @"SELECT 
                        r.id as Id,
                        r.titulo as Titulo,
                        r.descripcion as Descripcion,
                        r.tiempo_preparacion as TiempoPreparacion,
                        r.pasos as Pasos,
                        r.categoria_id as CategoriaId
                    FROM recetas r
                    INNER JOIN categorias c ON r.categoria_id = c.id
                    WHERE LOWER(c.nombre) = LOWER(@CategoriaNombre)"; 

        var recetas = (await connection.QueryAsync<Receta>(sqlRecetas, new { CategoriaNombre = categoriaNombre })).ToList();
        
        foreach (var receta in recetas)
        {
            var sqlIngredientes = @"SELECT 
                                  ri.ingrediente_id,
                                  ri.cantidad,
                                  ri.unidad_medida_id,
                                  um.nombre as unidad_nombre,
                                  um.abreviatura as unidad_abreviatura,
                                  i.nombre
                              FROM receta_ingredientes ri
                              INNER JOIN ingredientes i ON ri.ingrediente_id = i.id
                              INNER JOIN unidades_medida um ON ri.unidad_medida_id = um.id
                              WHERE ri.receta_id = @RecetaId";

            var ingredientesData = await connection.QueryAsync<dynamic>(sqlIngredientes, new { RecetaId = receta.Id });

            receta.RecetaIngredientes = ingredientesData.Select(item => new RecetaIngrediente
            {
                RecetaId = receta.Id,
                IngredienteId = (int)item.ingrediente_id,
                Cantidad = item.cantidad ?? 0m,
                UnidadMedidaId = (int)item.unidad_medida_id,
                UnidadMedida = new UnidadMedida
                {
                    Id = (int)item.unidad_medida_id,
                    Nombre = item.unidad_nombre ?? "",
                    Abreviatura = item.unidad_abreviatura ?? ""
                },
                Ingrediente = new Ingrediente
                {
                    Id = (int)item.ingrediente_id,
                    Nombre = item.nombre ?? ""
                }
            }).ToList();
        }

        return recetas;
    }
    public async Task<Receta?> ObtenerPorIdAsync(int id)
    { using var connection = _context.CreateConnection();
        var sqlReceta = @"SELECT 
                id as Id,
                titulo as Titulo,
                descripcion as Descripcion,
                tiempo_preparacion as TiempoPreparacion,
                pasos as Pasos,
                categoria_id as CategoriaId
                FROM recetas 
                WHERE id = @Id";

        var receta = await connection.QueryFirstOrDefaultAsync<Receta>(sqlReceta, new { Id = id });
        if (receta == null) return null;
        if (receta.CategoriaId > 0)
        {
            var sqlCategoria = @"SELECT 
                        id as Id,
                        nombre as Nombre
                    FROM categorias 
                    WHERE id = @CategoriaId";

            var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(
                sqlCategoria,
                new { CategoriaId = receta.CategoriaId }
            );

            receta.Categoria = categoria;
        }
        var sqlIngredientes = @"SELECT 
                      ri.ingrediente_id,
                      ri.cantidad,
                      ri.unidad_medida_id,
                      um.nombre as unidad_nombre,
                      um.abreviatura as unidad_abreviatura,
                      i.nombre
                  FROM receta_ingredientes ri
                  INNER JOIN ingredientes i ON ri.ingrediente_id = i.id
                  INNER JOIN unidades_medida um ON ri.unidad_medida_id = um.id
                  WHERE ri.receta_id = @RecetaId";

        var ingredientesData = await connection.QueryAsync<dynamic>(sqlIngredientes, new { RecetaId = id });

        receta.RecetaIngredientes = ingredientesData.Select(item => new RecetaIngrediente
        {
            RecetaId = receta.Id,
            IngredienteId = (int)item.ingrediente_id,
            Cantidad = item.cantidad ?? 0m,
            UnidadMedidaId = (int)item.unidad_medida_id,
            UnidadMedida = new UnidadMedida
            {
                Id = (int)item.unidad_medida_id,
                Nombre = item.unidad_nombre ?? "",
                Abreviatura = item.unidad_abreviatura ?? ""
            },
            Ingrediente = new Ingrediente
            {
                Id = (int)item.ingrediente_id,
                Nombre = item.nombre ?? ""
            }
        }).ToList();

        return receta;
    }

    public async Task<Receta> CrearAsync(Receta receta)
    {
        using var connection = _context.CreateConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var sqlReceta = @"INSERT INTO recetas (titulo, descripcion, tiempo_preparacion, pasos, categoria_id) 
                      VALUES (@Titulo, @Descripcion, @TiempoPreparacion, @Pasos, @CategoriaId) 
                      RETURNING id";

            var id = await connection.ExecuteScalarAsync<int>(
                sqlReceta,
                receta,
                transaction: transaction
            );
            receta.Id = id;

            if (receta.RecetaIngredientes != null && receta.RecetaIngredientes.Any())
            {
                var sqlIngredientes = @"INSERT INTO receta_ingredientes (receta_id, ingrediente_id, cantidad, unidad_medida_id) 
                                VALUES (@RecetaId, @IngredienteId, @Cantidad, @UnidadMedidaId)";

                foreach (var ingrediente in receta.RecetaIngredientes)
                {
                    await connection.ExecuteAsync(
                        sqlIngredientes,
                        new
                        {
                            RecetaId = id,
                            IngredienteId = ingrediente.IngredienteId,
                            Cantidad = ingrediente.Cantidad,
                            UnidadMedidaId = ingrediente.UnidadMedidaId
                        },
                        transaction: transaction
                    );
                }
            }
            transaction.Commit();

            return receta;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task ActualizarAsync(Receta receta)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE recetas 
                    SET titulo = @Titulo, 
                        descripcion = @Descripcion, 
                        tiempo_preparacion = @TiempoPreparacion, 
                        pasos = @Pasos, 
                        categoria_id = @CategoriaId 
                        WHERE id = @Id";
        await connection.ExecuteAsync(sql, receta);
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var sqlIngredientes = @"DELETE FROM receta_ingredientes WHERE receta_id = @Id";
        await connection.ExecuteAsync(sqlIngredientes, new { Id = id });
        var sqlReceta = @"DELETE FROM recetas WHERE id = @Id";
        await connection.ExecuteAsync(sqlReceta, new { Id = id });
    }
    public async Task<Receta?> ObtenerPorIdConRelacionesAsync(int id)
    { using var connection = _context.CreateConnection();
        var sqlReceta = @"SELECT 
            id as Id,
            titulo as Titulo,
            descripcion as Descripcion,
            tiempo_preparacion as TiempoPreparacion,
            pasos as Pasos,
            categoria_id as CategoriaId
            FROM recetas 
            WHERE id = @Id";

        var receta = await connection.QueryFirstOrDefaultAsync<Receta>(sqlReceta, new { Id = id });
        if (receta == null) return null;
        if (receta.CategoriaId > 0)
        {
            var sqlCategoria = @"SELECT 
                    id as Id,
                    nombre as Nombre
                    FROM categorias 
                    WHERE id = @CategoriaId";

            var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(
                sqlCategoria,
                new { CategoriaId = receta.CategoriaId }
            );
            receta.Categoria = categoria;
        }
        var sqlIngredientes = @"SELECT 
                  ri.ingrediente_id,
                  ri.cantidad,
                  ri.unidad_medida_id,
                  um.nombre as unidad_nombre,
                  um.abreviatura as unidad_abreviatura,
                  i.nombre
                  FROM receta_ingredientes ri
                  INNER JOIN ingredientes i ON ri.ingrediente_id = i.id
                  INNER JOIN unidades_medida um ON ri.unidad_medida_id = um.id
                  WHERE ri.receta_id = @RecetaId";

        var ingredientesData = await connection.QueryAsync<dynamic>(sqlIngredientes, new { RecetaId = id });

        receta.RecetaIngredientes = ingredientesData.Select(item => new RecetaIngrediente
        {
            RecetaId = receta.Id,
            IngredienteId = (int)item.ingrediente_id,
            Cantidad = item.cantidad ?? 0m,
            UnidadMedidaId = (int)item.unidad_medida_id,
            UnidadMedida = new UnidadMedida
            {
                Id = (int)item.unidad_medida_id,
                Nombre = item.unidad_nombre ?? "",
                Abreviatura = item.unidad_abreviatura ?? ""
            },
            Ingrediente = new Ingrediente
            {
                Id = (int)item.ingrediente_id,
                Nombre = item.nombre ?? ""
            }
        }).ToList();
        var sqlComentarios = @"SELECT 
                  id,
                  texto,
                  calificacion,
                  receta_id,
                  nombre_usuario,
                  fecha_creacion
                  FROM comentarios 
                  WHERE receta_id = @RecetaId";

        var comentarios = await connection.QueryAsync<Comentario>(sqlComentarios, new { RecetaId = id });
        receta.Comentarios = comentarios.ToList();
        Console.WriteLine($"=== DIAGNÓSTICO Repositorio ===");
        Console.WriteLine($"Receta ID: {receta.Id}, Título: {receta.Titulo}");
        Console.WriteLine($"Comentarios encontrados: {receta.Comentarios?.Count ?? 0}");

        if (receta.Comentarios != null && receta.Comentarios.Any())
        {
            Console.WriteLine("Detalles de comentarios:");
            foreach (var comentario in receta.Comentarios)
            {
                Console.WriteLine($"  - ID: {comentario.Id}, Calificación: {comentario.Calificacion}");
            }
        }
        Console.WriteLine($"=== FIN DIAGNÓSTICO ===");

        return receta;
    }
    public async Task<List<Receta>> ObtenerTodasConComentariosAsync()
    { using var connection = _context.CreateConnection();
        var sqlRecetas = @"SELECT 
                id as Id,
                titulo as Titulo,
                descripcion as Descripcion,
                tiempo_preparacion as TiempoPreparacion,
                pasos as Pasos,
                categoria_id as CategoriaId
            FROM recetas";

        var recetas = (await connection.QueryAsync<Receta>(sqlRecetas)).ToList();

        foreach (var receta in recetas)
        { if (receta.CategoriaId > 0)
            {  var sqlCategoria = @"SELECT 
                        id as Id,
                        nombre as Nombre
                    FROM categorias 
                    WHERE id = @CategoriaId";
                var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(
                    sqlCategoria, new { CategoriaId = receta.CategoriaId });
                receta.Categoria = categoria;
            }
            var sqlComentarios = @"SELECT 
                      id,
                      texto,
                      calificacion,
                      receta_id,
                      nombre_usuario,
                      fecha_creacion
                  FROM comentarios 
                  WHERE receta_id = @RecetaId";
            var comentarios = await connection.QueryAsync<Comentario>(sqlComentarios, new { RecetaId = receta.Id });
            receta.Comentarios = comentarios.ToList();
            var sqlIngredientes = @"SELECT 
                          ri.ingrediente_id,
                          ri.cantidad,
                          ri.unidad_medida_id,
                          um.nombre as unidad_nombre,
                          um.abreviatura as unidad_abreviatura,
                          i.nombre
                      FROM receta_ingredientes ri
                      INNER JOIN ingredientes i ON ri.ingrediente_id = i.id
                      INNER JOIN unidades_medida um ON ri.unidad_medida_id = um.id
                      WHERE ri.receta_id = @RecetaId";

            var ingredientesData = await connection.QueryAsync<dynamic>(sqlIngredientes, new { RecetaId = receta.Id });

            receta.RecetaIngredientes = ingredientesData.Select(item => new RecetaIngrediente
            {
                RecetaId = receta.Id,
                IngredienteId = (int)item.ingrediente_id,
                Cantidad = item.cantidad ?? 0m,
                UnidadMedidaId = (int)item.unidad_medida_id,
                UnidadMedida = new UnidadMedida
                {
                    Id = (int)item.unidad_medida_id,
                    Nombre = item.unidad_nombre ?? "",
                    Abreviatura = item.unidad_abreviatura ?? ""
                },
                Ingrediente = new Ingrediente
                {
                    Id = (int)item.ingrediente_id,
                    Nombre = item.nombre ?? ""
                }
            }).ToList();
        }
        return recetas;
    }
}