using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de predicciones usando Entity Framework Core.
/// </summary>
public class PredictionRepository : IPredictionRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de predicciones.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public PredictionRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Prediction?> GetByUserAndMatchAsync(int userId, int matchId)
    {
        return await _context.Predictions
            .Include(p => p.Match)
            .FirstOrDefaultAsync(p => p.IdUser == userId && p.IdMatch == matchId && p.State);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Prediction>> GetAllByUserAsync(int userId)
    {
        return await _context.Predictions
            .Include(p => p.Match)
                .ThenInclude(m => m.MatchResult)
            .Where(p => p.IdUser == userId && p.State)
            .OrderBy(p => p.Match.MatchDate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Prediction>> GetAllByMatchAsync(int matchId)
    {
        return await _context.Predictions
            .Where(p => p.IdMatch == matchId && p.State)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Prediction> CreateAsync(Prediction prediction)
    {
        _context.Predictions.Add(prediction);
        await _context.SaveChangesAsync();
        return prediction;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Prediction prediction)
    {
        _context.Predictions.Update(prediction);
        await _context.SaveChangesAsync();
    }
}
