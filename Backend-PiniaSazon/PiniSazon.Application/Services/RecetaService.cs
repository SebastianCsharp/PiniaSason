using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Services;

public class RecetaService : IRecetaService
{
    private readonly IRecetaRepository _recetaRepository;

    public RecetaService(IRecetaRepository recetaRepository)
    {
        _recetaRepository = recetaRepository;
    }
    public async Task<List<RecetaDto>> ObtenerTodasRecetasAsync()
    {
        var recetas = await _recetaRepository.ObtenerTodasAsync();

        return recetas.Select(receta => new RecetaDto
        {Id = receta.Id,
            Titulo = receta.Titulo,
            Descripcion = receta.Descripcion,
            TiempoPreparacion = receta.TiempoPreparacion,
            Pasos = receta.Pasos,
            CategoriaNombre = receta.Categoria?.Nombre ?? "",
            CalificacionPromedio = receta.Comentarios.Any()
                ? (decimal)receta.Comentarios.Average(c => c.Calificacion)
                : 0,
            Ingredientes = receta.RecetaIngredientes.Select(ri => new IngredienteDto
            {
                Nombre = ri.Ingrediente.Nombre,
                Cantidad = ri.Cantidad,
                UnidadMedida = ri.UnidadMedida?.Abreviatura ?? ""
            }).ToList()
        }).ToList();
    }
    public async Task<List<RecetaDto>> ObtenerRecetaPorCategoriaAsync(string categoriaNombre)
    {
        var recetas = await _recetaRepository.ObtenerPorCategoriaAsync(categoriaNombre);
        return recetas.Select(receta => new RecetaDto
        { Id = receta.Id,
            Titulo = receta.Titulo,
            Descripcion = receta.Descripcion,
            TiempoPreparacion = receta.TiempoPreparacion,
            Pasos = receta.Pasos,
            CategoriaNombre = receta.Categoria?.Nombre ?? "",
            CalificacionPromedio = receta.Comentarios.Any()
                ? (decimal)receta.Comentarios.Average(c => c.Calificacion)
                : 0,
            Ingredientes = receta.RecetaIngredientes.Select(ri => new IngredienteDto
            {
                Nombre = ri.Ingrediente.Nombre,
                Cantidad = ri.Cantidad,
                UnidadMedida = ri.UnidadMedida?.Abreviatura ?? ""
            }).ToList()
        }).ToList();
    }
    public async Task<RecetaDto?> ObtenerRecetaPorIdAsync(int id)
    { var receta = await _recetaRepository.ObtenerPorIdConRelacionesAsync(id);

        if (receta == null)
            return null;

        return new RecetaDto
        {
            Id = receta.Id,
            Titulo = receta.Titulo,
            Descripcion = receta.Descripcion,
            TiempoPreparacion = receta.TiempoPreparacion,
            Pasos = receta.Pasos,
            CategoriaNombre = receta.Categoria?.Nombre ?? "",
            CalificacionPromedio = receta.Comentarios.Any()
                ? (decimal)receta.Comentarios.Average(c => c.Calificacion)
                : 0,
            Ingredientes = receta.RecetaIngredientes?.Select(ri => new IngredienteDto
            {
                Nombre = ri.Ingrediente?.Nombre ?? "",
                Cantidad = ri.Cantidad,
                UnidadMedida = ri.UnidadMedida?.Abreviatura ?? ""
            }).ToList() ?? new List<IngredienteDto>()
        };
    }

    public async Task<RecetaDto> CrearRecetaAsync(CrearRecetaDto crearRecetaDto)
    {   // validaciones 
     
        if (string.IsNullOrWhiteSpace(crearRecetaDto.Titulo))
        {
            throw new ArgumentException("El título de la receta es obligatorio.");
        }
        if (string.IsNullOrWhiteSpace(crearRecetaDto.Descripcion))
        {
            throw new ArgumentException("La descripción de la receta es obligatoria.");
        }
        if (crearRecetaDto.TiempoPreparacion <= 0)
        {
            throw new ArgumentException("El tiempo de preparación debe ser mayor a 0.");
        }
        if (string.IsNullOrWhiteSpace(crearRecetaDto.Pasos))
        {
            throw new ArgumentException("Los pasos de preparación son obligatorios.");
        }
        if (crearRecetaDto.CategoriaId <= 0)
        {
            throw new ArgumentException("Debe seleccionar una categoría para la receta.");
        }
        if (crearRecetaDto.Ingredientes == null || !crearRecetaDto.Ingredientes.Any())
        {
            throw new ArgumentException("Una receta debe tener al menos un ingrediente.");
        }
        var receta = new Receta
        {
            Titulo = crearRecetaDto.Titulo,
            Descripcion = crearRecetaDto.Descripcion,
            TiempoPreparacion = crearRecetaDto.TiempoPreparacion,
            Pasos = crearRecetaDto.Pasos,
            CategoriaId = crearRecetaDto.CategoriaId
        };

        receta.RecetaIngredientes = new List<RecetaIngrediente>();

        foreach (var ingredienteDto in crearRecetaDto.Ingredientes)
        {
            receta.RecetaIngredientes.Add(new RecetaIngrediente
            {
                IngredienteId = ingredienteDto.IngredienteId,
                Cantidad = ingredienteDto.Cantidad,
                UnidadMedidaId = ingredienteDto.UnidadMedidaId
            });
        }

        await _recetaRepository.CrearAsync(receta);

        return new RecetaDto
        {
            Id = receta.Id,
            Titulo = receta.Titulo,
            Descripcion = receta.Descripcion,
            TiempoPreparacion = receta.TiempoPreparacion,
            Pasos = receta.Pasos,
            CategoriaNombre = "",
            CalificacionPromedio = 0,
            Ingredientes = crearRecetaDto.Ingredientes.Select(i => new IngredienteDto
            {
                Nombre = "",
                Cantidad = i.Cantidad,
                UnidadMedida = i.UnidadMedidaId.ToString()
            }).ToList()
        };
    }
    public async Task<bool> EliminarRecetaAsync(int id)
    {
        
        var receta = await _recetaRepository.ObtenerPorIdAsync(id);
        if (receta == null)
            return false;  
        await _recetaRepository.EliminarAsync(id);
        return true; 
    }
    public async Task<RecetaDto?> ActualizarRecetaAsync(int id, ActualizarRecetaDto actualizarRecetaDto)
    {  var recetaExistente = await _recetaRepository.ObtenerPorIdAsync(id);
        if (recetaExistente == null)
            return null;

        recetaExistente.Titulo = actualizarRecetaDto.Titulo;
        recetaExistente.Descripcion = actualizarRecetaDto.Descripcion;
        recetaExistente.TiempoPreparacion = actualizarRecetaDto.TiempoPreparacion;
        recetaExistente.Pasos = actualizarRecetaDto.Pasos;
        recetaExistente.CategoriaId = actualizarRecetaDto.CategoriaId;

        await _recetaRepository.ActualizarAsync(recetaExistente);

        
        var recetaActualizada = await _recetaRepository.ObtenerPorIdAsync(id);

        return new RecetaDto
        {
            Id = recetaActualizada.Id,
            Titulo = recetaActualizada.Titulo,
            Descripcion = recetaActualizada.Descripcion,
            TiempoPreparacion = recetaActualizada.TiempoPreparacion,
            Pasos = recetaActualizada.Pasos,
            CategoriaNombre = recetaActualizada.Categoria?.Nombre ?? "",
            CalificacionPromedio = recetaActualizada.Comentarios.Any()
                ? (decimal)recetaActualizada.Comentarios.Average(c => c.Calificacion)
                : 0,
            Ingredientes = recetaActualizada.RecetaIngredientes.Select(ri => new IngredienteDto
            {
                Nombre = ri.Ingrediente.Nombre,
                Cantidad = ri.Cantidad,
                UnidadMedida = ri.UnidadMedida?.Abreviatura ?? ""
            }).ToList()
        };
    }
   
    public async Task<List<RecetaDto>> ObtenerRecetasDestacadasAsync()
    {var recetas = await _recetaRepository.ObtenerTodasConComentariosAsync();
        var recetasDestacadas = recetas
            .Where(r => r.Comentarios != null && r.Comentarios.Any())
            .Where(r => r.Comentarios.Average(c => c.Calificacion) >= 4)
            .OrderByDescending(r => r.Comentarios.Average(c => c.Calificacion))
            .Take(6)
            .ToList();

        return recetasDestacadas.Select(receta => new RecetaDto
        {
            Id = receta.Id,
            Titulo = receta.Titulo,
            Descripcion = receta.Descripcion,
            TiempoPreparacion = receta.TiempoPreparacion,
            Pasos = receta.Pasos,
            CategoriaNombre = receta.Categoria?.Nombre ?? "",
            CalificacionPromedio = receta.Comentarios.Any()
                ? (decimal)receta.Comentarios.Average(c => c.Calificacion)
                : 0,
            Ingredientes = receta.RecetaIngredientes?.Select(ri => new IngredienteDto
            {
                Nombre = ri.Ingrediente?.Nombre ?? "",
                Cantidad = ri.Cantidad,
                UnidadMedida = ri.UnidadMedida?.Abreviatura ?? ""
            }).ToList() ?? new List<IngredienteDto>()
        }).ToList();
    }
}
