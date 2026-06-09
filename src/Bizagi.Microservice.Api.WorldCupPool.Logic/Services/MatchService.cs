using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Match;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de partidos.
/// </summary>
public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de partidos.
    /// </summary>
    /// <param name="matchRepository">Repositorio de partidos.</param>
    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatchDto>> GetAllAsync()
    {
        var matches = await _matchRepository.GetAllActiveAsync();

        return matches.Select(m => new MatchDto
        {
            Id = m.Id,
            IdGroup = m.IdGroup,
            GroupName = m.Group?.Name ?? string.Empty,
            IdHomeTeam = m.IdHomeTeam,
            HomeTeamName = m.HomeTeamName,
            IdAwayTeam = m.IdAwayTeam,
            AwayTeamName = m.AwayTeamName,
            MatchDate = m.MatchDate,
            RoundName = m.RoundName,
            Status = m.Status,
            // Resultado real — null si el partido aún no ha finalizado
            RealHomeGoals = m.MatchResult?.HomeGoals,
            RealAwayGoals = m.MatchResult?.AwayGoals,
        });
    }
}