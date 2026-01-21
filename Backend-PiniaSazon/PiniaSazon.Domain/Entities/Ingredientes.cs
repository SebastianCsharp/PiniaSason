using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Entities;

public class Ingrediente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
}