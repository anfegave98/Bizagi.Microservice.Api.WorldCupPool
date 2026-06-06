using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de partidos usando Entity Framework Core.
/// </summary>
public class MatchRepository : IMatchRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de partidos.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public MatchRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Match>> GetAllActiveAsync()
    {
        return await _context.Matches
            .Include(m => m.Group)
            .Where(m => m.State)
            .OrderBy(m => m.MatchDate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Match?> GetByIdAsync(decimal matchId)
    {
        return await _context.Matches
            .Include(m => m.Group)
            .FirstOrDefaultAsync(m => m.Id == matchId && m.State);
    }

    /// <inheritdoc />
    public async Task UpdateStatusAsync(decimal matchId, string status)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match is not null)
        {
            match.Status = status;
            match.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
