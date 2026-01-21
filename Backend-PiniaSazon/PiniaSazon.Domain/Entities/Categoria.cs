using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PiniaSazon.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Receta> Recetas { get; set; } = new List<Receta>();
    public int CantidadRecetas { get; set; }
}
