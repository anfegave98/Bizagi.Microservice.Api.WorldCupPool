using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de leaderboard global e historial de predicciones por usuario.
/// Delega las consultas optimizadas al repositorio especializado.
/// </summary>
public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepository;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de leaderboard.
    /// </summary>
    /// <param name="leaderboardRepository">Repositorio de leaderboard.</param>
    public LeaderboardService(ILeaderboardRepository leaderboardRepository)
    {
        _leaderboardRepository = leaderboardRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LeaderboardItemDto>> GetLeaderboardAsync()
    {
        return await _leaderboardRepository.GetLeaderboardAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserPredictionHistoryDto>> GetUserHistoryAsync(int userId)
    {
        return await _leaderboardRepository.GetUserHistoryAsync(userId);
    }
}
