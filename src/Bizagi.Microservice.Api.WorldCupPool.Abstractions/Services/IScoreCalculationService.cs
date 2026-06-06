namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de cálculo de puntuación.
/// </summary>
public interface IScoreCalculationService
{
    /// <summary>
    /// Calcula y actualiza los puntos de todas las predicciones activas para un partido,
    /// basándose en el resultado real registrado. Registra trazabilidad en ScoreLogs.
    /// </summary>
    /// <param name="matchId">Identificador del partido finalizado.</param>
    /// <param name="matchResultId">Identificador del resultado real registrado.</param>
    /// <param name="realHomeGoals">Goles reales del equipo local.</param>
    /// <param name="realAwayGoals">Goles reales del equipo visitante.</param>
    Task CalculateAsync(decimal matchId, decimal matchResultId, int realHomeGoals, int realAwayGoals);
}
