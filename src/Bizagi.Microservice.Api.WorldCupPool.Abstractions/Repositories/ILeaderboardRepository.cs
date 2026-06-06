using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de leaderboard.
/// Encapsula las consultas de ranking global e historial de predicciones por usuario,
/// ejecutadas directamente sobre el contexto para optimizar las proyecciones.
/// </summary>
public interface ILeaderboardRepository
{
    /// <summary>
    /// Obtiene el ranking global de usuarios, ordenado por puntos totales descendente.
    /// En caso de empate en puntos, se ordena por cantidad de marcadores exactos
    /// descendente y luego por nombre de usuario ascendente.
    /// Solo se consideran predicciones calculadas de partidos finalizados.
    /// </summary>
    /// <returns>Lista de filas del ranking con posición asignada.</returns>
    Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync();

    /// <summary>
    /// Obtiene el historial completo de predicciones de un usuario específico,
    /// incluyendo el resultado real del partido y los puntos obtenidos en cada predicción.
    /// </summary>
    /// <param name="userId">Identificador del usuario cuyo historial se consulta.</param>
    /// <returns>Lista de entradas del historial ordenadas por fecha de partido ascendente.</returns>
    Task<IEnumerable<UserPredictionHistoryDto>> GetUserHistoryAsync(decimal userId);
}
