using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Prediction;
using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de predicciones.
/// </summary>
public class PredictionService : IPredictionService
{
    private readonly IPredictionRepository _predictionRepository;
    private readonly IMatchRepository _matchRepository;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de predicciones.
    /// </summary>
    /// <param name="predictionRepository">Repositorio de predicciones.</param>
    /// <param name="matchRepository">Repositorio de partidos.</param>
    public PredictionService(
        IPredictionRepository predictionRepository,
        IMatchRepository matchRepository)
    {
        _predictionRepository = predictionRepository;
        _matchRepository = matchRepository;
    }

    /// <inheritdoc />
    public async Task<PredictionDto> CreateOrUpdateAsync(PredictionCreateDto dto, decimal userId)
    {
        var match = await _matchRepository.GetByIdAsync(dto.IdMatch)
            ?? throw new KeyNotFoundException($"El partido con Id {dto.IdMatch} no existe o no está disponible.");

        if (match.Status == MatchStatus.Finished)
            throw new InvalidOperationException("No se puede registrar o modificar una predicción para un partido que ya ha finalizado.");

        var existing = await _predictionRepository.GetByUserAndMatchAsync(userId, dto.IdMatch);

        Prediction prediction;

        if (existing is null)
        {
            prediction = new Prediction
            {
                IdUser = userId,
                IdMatch = dto.IdMatch,
                HomeGoals = dto.HomeGoals,
                AwayGoals = dto.AwayGoals,
                Points = 0,
                IsCalculated = false,
                State = true,
                IdUserCreator = (int)userId,
                DateCreated = DateTime.UtcNow
            };
            prediction = await _predictionRepository.CreateAsync(prediction);
        }
        else
        {
            existing.HomeGoals = dto.HomeGoals;
            existing.AwayGoals = dto.AwayGoals;
            existing.DateModified = DateTime.UtcNow;
            await _predictionRepository.UpdateAsync(existing);
            prediction = existing;
        }

        return MapToDto(prediction, match);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PredictionDto>> GetMineAsync(decimal userId)
    {
        var predictions = await _predictionRepository.GetAllByUserAsync(userId);

        return predictions.Select(p => MapToDto(p, p.Match));
    }

    private static PredictionDto MapToDto(Prediction prediction, Entities.Match match)
    {
        return new PredictionDto
        {
            Id = prediction.Id,
            IdUser = prediction.IdUser,
            IdMatch = prediction.IdMatch,
            HomeTeamName = match.HomeTeamName,
            AwayTeamName = match.AwayTeamName,
            HomeGoals = prediction.HomeGoals,
            AwayGoals = prediction.AwayGoals,
            RealHomeGoals = match.MatchResult?.HomeGoals,
            RealAwayGoals = match.MatchResult?.AwayGoals,
            Points = prediction.Points,
            IsCalculated = prediction.IsCalculated,
            MatchStatus = match.Status,
            DateCreated = prediction.DateCreated,
            DateModified = prediction.DateModified
        };
    }
}
