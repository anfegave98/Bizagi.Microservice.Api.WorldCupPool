using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador de partidos. Expone los partidos disponibles del mundial para consulta.
/// Requiere autenticación JWT.
/// </summary>
[ApiController]
[Route("api/v1/matches")]
[Authorize]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de partidos.
    /// </summary>
    /// <param name="matchService">Servicio de partidos.</param>
    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    /// <summary>
    /// Obtiene la lista de todos los partidos activos ordenados por fecha ascendente.
    /// </summary>
    /// <returns>Lista de partidos del mundial.</returns>
    /// <response code="200">Lista de partidos retornada exitosamente.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var matches = await _matchService.GetAllAsync();
        return Ok(matches);
    }
}
