
using System.ComponentModel.DataAnnotations;


namespace PiniaSazon.Application.Dtos
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty; 
        public string Texto { get; set; } = string.Empty;
        public int Calificacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
    public class CrearComentarioDto
    {
        // nombre usuario opcional
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreUsuario { get; set; } = "Anónimo"; 

        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
        public string Texto { get; set; } = string.Empty;

        // calificacion de 1 a 5 
        [Required(ErrorMessage = "La calificación es requerida")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int Calificacion { get; set; }
        public int RecetaId { get; set; }
    }
}
