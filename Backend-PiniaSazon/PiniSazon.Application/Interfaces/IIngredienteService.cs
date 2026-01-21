using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiniaSazon.Application.Dtos;

namespace PiniaSazon.Application.Interfaces;

public interface IIngredienteService
{
    Task<List<IngredienteDto>> ObtenerTodosAsync();
}