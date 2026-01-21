using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiniaSazon.Domain.Entities;

namespace PiniaSazon.Domain.Interfaces;

public interface IIngredienteRepository
{
    Task<List<Ingrediente>> ObtenerTodosAsync();
    Task<Ingrediente?> ObtenerPorIdAsync(int id);
    Task<Ingrediente> CrearAsync(Ingrediente ingrediente);
}
