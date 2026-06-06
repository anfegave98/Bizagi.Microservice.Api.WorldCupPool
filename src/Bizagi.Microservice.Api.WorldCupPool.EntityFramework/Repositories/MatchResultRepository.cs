using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de resultados de partidos usando Entity Framework Core.
/// </summary>
public class MatchResultRepository : IMatchResultRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de resultados de partidos.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public MatchResultRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByMatchIdAsync(decimal matchId)
    {
        return await _context.MatchResults
            .AnyAsync(mr => mr.IdMatch == matchId && mr.State);
    }

    /// <inheritdoc />
    public async Task<MatchResult> CreateAsync(MatchResult matchResult)
    {
        _context.MatchResults.Add(matchResult);
        await _context.SaveChangesAsync();
        return matchResult;
    }
}
