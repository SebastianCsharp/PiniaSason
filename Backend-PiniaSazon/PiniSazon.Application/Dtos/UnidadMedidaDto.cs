using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Dtos
{
    public class UnidadMedidaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Abreviatura {  get; set; } = string.Empty;
    }
}
