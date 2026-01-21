using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiniaSazon.Domain.Entities;

namespace PiniaSazon.Domain.Interfaces;

public interface IRecetaRepository
{
    Task<List<Receta>> ObtenerTodasAsync();
    Task<List<Receta>> ObtenerPorCategoriaAsync(string categoriaNombre);
    Task<Receta> CrearAsync(Receta receta);
    Task ActualizarAsync(Receta receta);
    Task EliminarAsync(int id);
    Task<Receta?> ObtenerPorIdAsync(int id);
    Task<Receta?> ObtenerPorIdConRelacionesAsync(int id);
    Task<List<Receta>> ObtenerTodasConComentariosAsync();
}
