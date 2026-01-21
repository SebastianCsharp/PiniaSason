using PiniaSazon.Application.Interfaces;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiniaSazon.Application.Services
{
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _unidadMedidaRepository;
        public UnidadMedidaService(IUnidadMedidaRepository unidadMedidaRepository)
        {
            _unidadMedidaRepository = unidadMedidaRepository;
        }

        public async Task<List<UnidadMedidaDto>> ObtenerTodasAsync()
        {var unidades = await _unidadMedidaRepository.ObtenerTodasAsync();
            var unidadesDto = unidades.Select(unidad => new UnidadMedidaDto
            {
                Id = unidad.Id,
                Nombre = unidad.Nombre,
                Abreviatura = unidad.Abreviatura
            }).ToList();
            return unidadesDto;
        }
    }
}