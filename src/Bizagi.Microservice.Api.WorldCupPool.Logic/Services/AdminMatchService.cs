using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;
using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio administrativo de partidos.
/// Permite registrar el resultado real de un partido y dispara el cálculo de puntos.
/// </summary>
public class AdminMatchService : IAdminMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchResultRepository _matchResultRepository;
    private readonly IScoreCalculationService _scoreCalculationService;

    /// <summary>
    /// Inicializa una nueva instancia del servicio administrativo de partidos.
    /// </summary>
    /// <param name="matchRepository">Repositorio de partidos.</param>
    /// <param name="matchResultRepository">Repositorio de resultados de partidos.</param>
    /// <param name="scoreCalculationService">Servicio de cálculo de puntuación.</param>
    public AdminMatchService(
        IMatchRepository matchRepository,
        IMatchResultRepository matchResultRepository,
        IScoreCalculationService scoreCalculationService)
    {
        _matchRepository = matchRepository;
        _matchResultRepository = matchResultRepository;
        _scoreCalculationService = scoreCalculationService;
    }

    /// <inheritdoc />
    public async Task<MatchResultDto> RegisterResultAsync(int matchId, MatchResultCreateDto dto, int adminUserId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException($"El partido con Id {matchId} no existe o no está disponible.");

        if (match.Status == MatchStatus.Finished)
            throw new InvalidOperationException("El partido ya tiene un resultado registrado.");

        if (await _matchResultRepository.ExistsByMatchIdAsync(matchId))
            throw new InvalidOperationException("Ya existe un resultado activo para este partido.");

        var matchResult = new MatchResult
        {
            IdMatch = matchId,
            HomeGoals = dto.HomeGoals,
            AwayGoals = dto.AwayGoals,
            RegisteredByUserId = adminUserId,
            RegisteredDate = DateTime.UtcNow,
            State = true,
            DateCreated = DateTime.UtcNow
        };

        var created = await _matchResultRepository.CreateAsync(matchResult);

        // Actualizar estado del partido a Finished
        await _matchRepository.UpdateStatusAsync(matchId, MatchStatus.Finished);

        // Disparar cálculo de puntos para todas las predicciones del partido
        await _scoreCalculationService.CalculateAsync(matchId, created.Id, dto.HomeGoals, dto.AwayGoals);

        return new MatchResultDto
        {
            Id = created.Id,
            IdMatch = created.IdMatch,
            HomeGoals = created.HomeGoals,
            AwayGoals = created.AwayGoals,
            RegisteredByUserId = created.RegisteredByUserId,
            RegisteredDate = created.RegisteredDate
        };
    }
}
