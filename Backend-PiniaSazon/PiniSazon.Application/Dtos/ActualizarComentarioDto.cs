using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Dtos
{
    public class ActualizarComentarioDto
    {
        [Required(ErrorMessage = "El texto del comentario es requerido")]
        [MinLength(1, ErrorMessage = "El comentario no puede estar vacío")]
        public string Texto { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5 estrellas")]
        public int Calificacion { get; set; }
    }
}
