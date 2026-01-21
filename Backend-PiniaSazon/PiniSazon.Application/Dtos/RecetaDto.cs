namespace PiniaSazon.Application.Dtos;

public class RecetaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int TiempoPreparacion { get; set; }
    public string Pasos { get; set; } = string.Empty;
    public string CategoriaNombre { get; set; } = string.Empty;
    public decimal CalificacionPromedio { get; set; }
    public List<IngredienteDto> Ingredientes { get; set; } = new List<IngredienteDto>();
}