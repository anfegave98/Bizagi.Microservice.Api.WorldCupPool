using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de trazabilidad de cálculo de puntajes usando Entity Framework Core.
/// </summary>
public class ScoreLogRepository : IScoreLogRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de ScoreLogs.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public ScoreLogRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CreateAsync(ScoreLog scoreLog)
    {
        _context.ScoreLogs.Add(scoreLog);
        await _context.SaveChangesAsync();
    }
}
