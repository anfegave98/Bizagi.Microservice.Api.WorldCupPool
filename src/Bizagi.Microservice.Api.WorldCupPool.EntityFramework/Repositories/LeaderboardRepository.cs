using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de leaderboard usando Entity Framework Core.
/// Proyecta directamente sobre el contexto para evitar cargar entidades completas en memoria.
/// </summary>
public class LeaderboardRepository : ILeaderboardRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de leaderboard.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public LeaderboardRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync()
    {
        // Se agrupan las predicciones calculadas de partidos finalizados por usuario.
        // Los usuarios sin predicciones calculadas no aparecen en el ranking.
        var rawRanking = await _context.Predictions
            .Where(p => p.State && p.IsCalculated)
            .GroupBy(p => p.IdUser)
            .Select(g => new
            {
                IdUser         = g.Key,
                TotalPoints    = g.Sum(p => p.Points),
                PredictionCount = g.Count(),
                ExactScoreCount = g.Count(p => p.Points == 3)
            })
            .OrderByDescending(x => x.TotalPoints)
            .ThenByDescending(x => x.ExactScoreCount)
            .ToListAsync();

        // Enriquecer con datos del usuario y asignar posición en memoria
        // (evita join complejo de ordenamiento+posición en SQL).
        var userIds = rawRanking.Select(r => r.IdUser).ToList();

        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id) && u.State)
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToListAsync();

        var userMap = users.ToDictionary(u => u.Id);

        var result = rawRanking
            .Where(r => userMap.ContainsKey(r.IdUser))
            .OrderByDescending(r => r.TotalPoints)
            .ThenByDescending(r => r.ExactScoreCount)
            .ThenBy(r => userMap[r.IdUser].UserName)
            .Select((r, index) => new LeaderboardItemDto
            {
                Position        = index + 1,
                IdUser          = r.IdUser,
                UserName        = userMap[r.IdUser].UserName,
                FullName        = userMap[r.IdUser].FullName,
                TotalPoints     = r.TotalPoints,
                PredictionCount = r.PredictionCount,
                ExactScoreCount = r.ExactScoreCount
            })
            .ToList();

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserPredictionHistoryDto>> GetUserHistoryAsync(decimal userId)
    {
        return await _context.Predictions
            .Where(p => p.IdUser == userId && p.State)
            .Include(p => p.Match)
                .ThenInclude(m => m.MatchResult)
            .OrderBy(p => p.Match.MatchDate)
            .Select(p => new UserPredictionHistoryDto
            {
                IdMatch              = p.IdMatch,
                HomeTeamName         = p.Match.HomeTeamName,
                AwayTeamName         = p.Match.AwayTeamName,
                PredictedHomeGoals   = p.HomeGoals,
                PredictedAwayGoals   = p.AwayGoals,
                RealHomeGoals        = p.Match.MatchResult != null ? p.Match.MatchResult.HomeGoals : (int?)null,
                RealAwayGoals        = p.Match.MatchResult != null ? p.Match.MatchResult.AwayGoals : (int?)null,
                Points               = p.Points,
                RuleApplied          = p.IsCalculated
                    ? _context.ScoreLogs
                        .Where(sl => sl.IdPrediction == p.Id && sl.State)
                        .Select(sl => sl.RuleApplied)
                        .FirstOrDefault() ?? string.Empty
                    : string.Empty,
                MatchStatus          = p.Match.Status,
                MatchDate            = p.Match.MatchDate
            })
            .ToListAsync();
    }
}
