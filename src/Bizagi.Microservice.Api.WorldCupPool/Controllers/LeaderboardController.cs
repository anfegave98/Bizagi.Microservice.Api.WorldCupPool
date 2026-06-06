using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador del leaderboard. Expone el ranking global de participantes
/// y el historial de predicciones de un usuario específico.
/// Requiere autenticación JWT con rol "User" o "Admin".
/// </summary>
[ApiController]
[Route("api/v1/leaderboard")]
[Authorize]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de leaderboard.
    /// </summary>
    /// <param name="leaderboardService">Servicio de leaderboard.</param>
    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    /// <summary>
    /// Obtiene el ranking global de todos los participantes, ordenado por puntos totales descendente.
    /// En caso de empate, se aplica criterio secundario por marcadores exactos y nombre de usuario.
    /// Solo se consideran puntos calculados de partidos finalizados.
    /// </summary>
    /// <returns>Lista de posiciones del ranking con datos del usuario y estadísticas.</returns>
    /// <response code="200">Ranking retornado exitosamente.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LeaderboardItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        var leaderboard = await _leaderboardService.GetLeaderboardAsync();
        return Ok(leaderboard);
    }

    /// <summary>
    /// Obtiene el historial completo de predicciones de un usuario específico.
    /// Incluye datos del partido, predicción realizada, resultado real y puntos obtenidos.
    /// Accesible por cualquier usuario autenticado, no solo el propio usuario.
    /// </summary>
    /// <param name="userId">Identificador del usuario cuyo historial se consulta.</param>
    /// <returns>Lista de entradas del historial del usuario, ordenadas por fecha de partido.</returns>
    /// <response code="200">Historial retornado exitosamente.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    [HttpGet("{userId:decimal}/history")]
    [ProducesResponseType(typeof(IEnumerable<UserPredictionHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserHistoryAsync([FromRoute] decimal userId)
    {
        var history = await _leaderboardService.GetUserHistoryAsync(userId);
        return Ok(history);
    }
}
