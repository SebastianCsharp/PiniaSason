using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Entities;

[Table("recetas")]
public class Receta
{
    [Column("id")]
    public int Id { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    [Column("tiempo_preparacion")]
    public int TiempoPreparacion { get; set; } // en minutos

    [Column("pasos")]
    public string Pasos { get; set; } = string.Empty;

    [Column("categoria_id")]
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}
