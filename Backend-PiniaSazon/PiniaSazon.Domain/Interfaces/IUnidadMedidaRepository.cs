using PiniaSazon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Interfaces
{
    public interface IUnidadMedidaRepository
    { Task<List<UnidadMedida>> ObtenerTodasAsync();
        Task<UnidadMedida?> ObtenerPorIdAsync(int id);
    }
}
