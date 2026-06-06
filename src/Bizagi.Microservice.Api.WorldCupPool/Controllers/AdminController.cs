using System.Security.Claims;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador administrativo. Expone operaciones exclusivas para el rol "Admin",
/// como el registro del resultado real de un partido.
/// </summary>
[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminMatchService _adminMatchService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador administrativo.
    /// </summary>
    /// <param name="adminMatchService">Servicio administrativo de partidos.</param>
    public AdminController(IAdminMatchService adminMatchService)
    {
        _adminMatchService = adminMatchService;
    }

    /// <summary>
    /// Registra el resultado real de un partido. Cambia el estado del partido a "Finished"
    /// y dispara el cálculo automático de puntos para todas las predicciones asociadas.
    /// Solo puede ser llamado una vez por partido.
    /// </summary>
    /// <param name="matchId">Identificador del partido a finalizar.</param>
    /// <param name="dto">Goles reales del partido (local y visitante).</param>
    /// <returns>Resultado registrado.</returns>
    /// <response code="200">Resultado registrado y puntos calculados exitosamente.</response>
    /// <response code="400">El partido ya tiene un resultado registrado o datos inválidos.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    /// <response code="403">El usuario no tiene el rol Admin.</response>
    /// <response code="404">El partido no existe.</response>
    [HttpPost("matches/{matchId:decimal}/result")]
    [ProducesResponseType(typeof(MatchResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterResult(
        [FromRoute] decimal matchId,
        [FromBody] MatchResultCreateDto dto)
    {
        var adminUserId = GetCurrentUserId();
        var result = await _adminMatchService.RegisterResultAsync(matchId, dto, adminUserId);
        return Ok(result);
    }

    /// <summary>
    /// Extrae el identificador del administrador autenticado desde los claims del token JWT.
    /// </summary>
    /// <returns>Id del administrador autenticado.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el claim no existe o es inválido.</exception>
    private decimal GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(sub) || !decimal.TryParse(sub, out var userId))
            throw new UnauthorizedAccessException("No se pudo identificar el administrador autenticado.");

        return userId;
    }
}
