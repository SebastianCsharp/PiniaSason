using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Domain.Entities;

[Table("comentarios")]
public class Comentario
{
    [Column("id")]
    public int Id { get; set; }

    [Column("receta_id")]
    public int RecetaId { get; set; }

    [Column("nombre_usuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    // comentario 
    [Column("texto")]
    public string Texto { get; set; } = string.Empty;

    [Column("calificacion")]
    public int Calificacion { get; set; }

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }

    public Receta Receta { get; set; } = null!;
}