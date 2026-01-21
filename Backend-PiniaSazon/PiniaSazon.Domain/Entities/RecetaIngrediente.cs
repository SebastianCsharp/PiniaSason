using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Entities;

public class RecetaIngrediente
{
    public int RecetaId { get; set; }
    public Receta Receta { get; set; } = null!;
    public int IngredienteId { get; set; }
    public Ingrediente Ingrediente { get; set; } = null!;
    public decimal Cantidad { get; set; }
    public int UnidadMedidaId { get; set; }
    public UnidadMedida UnidadMedida { get; set; } = null !;
}
