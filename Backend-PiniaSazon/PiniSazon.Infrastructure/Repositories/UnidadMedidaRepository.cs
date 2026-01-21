using Dapper;
using PiniaSazon.Domain.Entities;
using PiniaSazon.Domain.Interfaces;
using PiniaSazon.Infrastructure.Data;


namespace PiniaSazon.Infrastructure.Repositories
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly DatabaseContext _context;
        public UnidadMedidaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<UnidadMedida>> ObtenerTodasAsync()
        {
            using var connection = _context.CreateConnection();

            var sql = @"SELECT * FROM unidades_medida ORDER BY nombre";

            var unidades = await connection.QueryAsync<UnidadMedida>(sql);

            return unidades.ToList();
        }

        public async Task<UnidadMedida?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            var sql = @"SELECT * FROM unidades_medida WHERE id = @Id";

            var unidad = await connection.QueryFirstOrDefaultAsync<UnidadMedida>(sql, new { Id = id });

            return unidad;
        }
    }
}
