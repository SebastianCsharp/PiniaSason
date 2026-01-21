using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Dtos;

public class CrearRecetaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int TiempoPreparacion { get; set; }
    public string Pasos { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public List<IngredienteRecetaDto> Ingredientes { get; set; } = new();
}