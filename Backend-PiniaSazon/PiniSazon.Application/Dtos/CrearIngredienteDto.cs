using System.ComponentModel.DataAnnotations;

namespace PiniaSazon.Application.Dtos;

public class CrearIngredienteDto
{
    [Required(ErrorMessage = "El nombre del ingrediente es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
}