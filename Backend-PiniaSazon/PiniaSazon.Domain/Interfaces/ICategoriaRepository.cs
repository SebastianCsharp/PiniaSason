using PiniaSazon.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace PiniaSazon.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>> ObtenerTodasAsync();
    Task<Categoria?> ObtenerPorIdAsync(int id);
    Task<Categoria> CrearAsync(Categoria categoria);
    Task ActualizarAsync(Categoria categoria);
    Task EliminarAsync(int id);
    Task<List<Categoria>> ObtenerCategoriasConConteoRecetasAsync();
}