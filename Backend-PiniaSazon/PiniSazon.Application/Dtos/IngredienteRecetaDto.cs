namespace PiniaSazon.Application.Dtos;

public class IngredienteRecetaDto
{
    public int IngredienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public int UnidadMedidaId { get; set; } 
}