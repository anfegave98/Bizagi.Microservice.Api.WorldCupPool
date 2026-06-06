using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.Extensions.Logging;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de cálculo de puntuación.
/// Reglas:
/// - Marcador exacto → 3 puntos.
/// - Acierto de ganador o empate → 1 punto.
/// - Fallo → 0 puntos.
/// </summary>
public class ScoreCalculationService : IScoreCalculationService
{
    private readonly IPredictionRepository _predictionRepository;
    private readonly IScoreLogRepository _scoreLogRepository;
    private readonly ILogger<ScoreCalculationService> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de cálculo de puntuación.
    /// </summary>
    /// <param name="predictionRepository">Repositorio de predicciones.</param>
    /// <param name="scoreLogRepository">Repositorio de logs de puntaje.</param>
    /// <param name="logger">Logger del servicio.</param>
    public ScoreCalculationService(
        IPredictionRepository predictionRepository,
        IScoreLogRepository scoreLogRepository,
        ILogger<ScoreCalculationService> logger)
    {
        _predictionRepository = predictionRepository;
        _scoreLogRepository = scoreLogRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task CalculateAsync(int matchId, int matchResultId, int realHomeGoals, int realAwayGoals)
    {
        _logger.LogInformation("Iniciando cálculo de puntos para el partido {MatchId}.", matchId);

        var predictions = await _predictionRepository.GetAllByMatchAsync(matchId);

        foreach (var prediction in predictions)
        {
            var (points, rule) = CalculatePoints(
                prediction.HomeGoals, prediction.AwayGoals,
                realHomeGoals, realAwayGoals);

            prediction.Points = points;
            prediction.IsCalculated = true;
            prediction.CalculatedDate = DateTime.UtcNow;
            prediction.DateModified = DateTime.UtcNow;

            await _predictionRepository.UpdateAsync(prediction);

            await _scoreLogRepository.CreateAsync(new ScoreLog
            {
                IdPrediction = prediction.Id,
                IdMatchResult = matchResultId,
                PredictedHomeGoals = prediction.HomeGoals,
                PredictedAwayGoals = prediction.AwayGoals,
                RealHomeGoals = realHomeGoals,
                RealAwayGoals = realAwayGoals,
                PointsAssigned = points,
                RuleApplied = rule,
                CalculationDate = DateTime.UtcNow,
                State = true,
                DateCreated = DateTime.UtcNow
            });
        }

        _logger.LogInformation(
            "Cálculo de puntos completado para el partido {MatchId}. {Count} predicciones procesadas.",
            matchId, predictions.Count());
    }

    /// <summary>
    /// Calcula los puntos y la regla aplicada para una predicción dado el resultado real.
    /// </summary>
    private static (int points, string rule) CalculatePoints(
        int predictedHome, int predictedAway,
        int realHome, int realAway)
    {
        // Marcador exacto: 3 puntos
        if (predictedHome == realHome && predictedAway == realAway)
            return (3, ScoreRule.ExactScore);

        // Acierto de ganador o empate: 1 punto
        var predictedResult = Math.Sign(predictedHome - predictedAway);
        var realResult = Math.Sign(realHome - realAway);

        if (predictedResult == realResult)
            return (1, ScoreRule.WinnerOrDraw);

        // Fallo: 0 puntos
        return (0, ScoreRule.Failed);
    }
}
