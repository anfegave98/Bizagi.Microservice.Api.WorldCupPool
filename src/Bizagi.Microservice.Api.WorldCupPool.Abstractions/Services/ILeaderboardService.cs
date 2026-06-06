using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de leaderboard global e historial de predicciones por usuario.
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Obtiene el ranking global de todos los participantes, ordenado por puntos totales descendente.
    /// En caso de empate, se ordena por cantidad de marcadores exactos y luego por nombre de usuario.
    /// </summary>
    /// <returns>Lista de posiciones del ranking con datos del usuario y estadísticas.</returns>
    Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync();

    /// <summary>
    /// Obtiene el historial completo de predicciones de un usuario específico,
    /// con la información del partido asociado, el resultado real y los puntos obtenidos.
    /// </summary>
    /// <param name="userId">Identificador del usuario cuyo historial se consulta.</param>
    /// <returns>Lista de entradas del historial del usuario.</returns>
    Task<IEnumerable<UserPredictionHistoryDto>> GetUserHistoryAsync(int userId);
}
