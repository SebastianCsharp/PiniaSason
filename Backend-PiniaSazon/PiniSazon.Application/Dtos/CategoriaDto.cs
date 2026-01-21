using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

    }
    public class CrearCategoriaDto
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class ActualizarCategoriaDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
    public class CategoriaConRecetasDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantidadRecetas { get; set; }
    }
}