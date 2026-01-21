using PiniaSazon.Application.Dtos;
using PiniaSazon.Application.Interfaces;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<CategoriaDto>> ObtenerTodasAsync()
    {
        var categorias = await _categoriaRepository.ObtenerTodasAsync();
        return categorias.Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre
        }).ToList();
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        var categoria = await _categoriaRepository.ObtenerPorIdAsync(id);
        if (categoria == null) return null;

        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre
        };
    }

    public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto crearCategoriaDto)
    {
        var categoria = new Categoria
        {
            Nombre = crearCategoriaDto.Nombre
        };

        var categoriaCreada = await _categoriaRepository.CrearAsync(categoria);
        return new CategoriaDto
        {
            Id = categoriaCreada.Id,
            Nombre = categoriaCreada.Nombre
        };
    }

    public async Task<CategoriaDto> ActualizarAsync(int id, ActualizarCategoriaDto actualizarCategoriaDto)
    {
        var categoriaExistente = await _categoriaRepository.ObtenerPorIdAsync(id);
        if (categoriaExistente == null) return null;

        categoriaExistente.Nombre = actualizarCategoriaDto.Nombre;
        await _categoriaRepository.ActualizarAsync(categoriaExistente);

        return new CategoriaDto
        {
            Id = categoriaExistente.Id,
            Nombre = categoriaExistente.Nombre
        };
    }
    public async Task<List<CategoriaConRecetasDto>> ObtenerCategoriasMasUsadasAsync()
    {
        var categoriasConConteo = await _categoriaRepository
            .ObtenerCategoriasConConteoRecetasAsync();

        var categoriasOrdenadas = categoriasConConteo
            .OrderByDescending(c => c.CantidadRecetas)
            .Take(4)
            .ToList();

        return categoriasOrdenadas.Select(c => new CategoriaConRecetasDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            CantidadRecetas = c.CantidadRecetas
        }).ToList();
    }

    public async Task EliminarAsync(int id)
    {
        await _categoriaRepository.EliminarAsync(id);
    }

}