using PiniaSazon.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodasAsync();
        Task<CategoriaDto> CrearAsync(CrearCategoriaDto crearCategoriaDto);
        Task<CategoriaDto> ActualizarAsync(int id, ActualizarCategoriaDto actualizarCategoriaDto);
        Task EliminarAsync(int id);
        Task<List<CategoriaConRecetasDto>> ObtenerCategoriasMasUsadasAsync();
    }

}