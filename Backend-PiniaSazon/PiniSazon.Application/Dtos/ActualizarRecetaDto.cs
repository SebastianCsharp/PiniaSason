using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Dtos
{
    public class ActualizarRecetaDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "El título no puede exceder 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(1, 500, ErrorMessage = "El tiempo de preparación debe ser entre 1 y 500 minutos")]
        public int TiempoPreparacion { get; set; }

        public string Pasos { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }
        public List<IngredienteRecetaDto> Ingredientes { get; set; } = new();
    }
}
