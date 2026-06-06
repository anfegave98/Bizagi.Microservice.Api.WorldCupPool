using System.Security.Claims;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Prediction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador de predicciones del usuario autenticado.
/// Permite registrar o actualizar predicciones y consultar las propias.
/// Requiere autenticación JWT con rol "User" o "Admin".
/// </summary>
[ApiController]
[Route("api/v1/predictions")]
[Authorize]
public class PredictionsController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de predicciones.
    /// </summary>
    /// <param name="predictionService">Servicio de predicciones.</param>
    public PredictionsController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    /// <summary>
    /// Crea o actualiza la predicción del usuario autenticado para un partido.
    /// No se permite predecir sobre partidos ya finalizados.
    /// </summary>
    /// <param name="dto">Datos de la predicción (IdMatch, HomeGoals, AwayGoals).</param>
    /// <returns>Predicción creada o actualizada.</returns>
    /// <response code="200">Predicción creada o actualizada exitosamente.</response>
    /// <response code="400">Partido finalizado o datos inválidos.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    /// <response code="404">El partido no existe.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PredictionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] PredictionCreateDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _predictionService.CreateOrUpdateAsync(dto, userId);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene todas las predicciones activas del usuario autenticado.
    /// </summary>
    /// <returns>Lista de predicciones del usuario con su estado y puntos.</returns>
    /// <response code="200">Lista de predicciones retornada exitosamente.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    [HttpGet("mine")]
    [ProducesResponseType(typeof(IEnumerable<PredictionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMine()
    {
        var userId = GetCurrentUserId();
        var predictions = await _predictionService.GetMineAsync(userId);
        return Ok(predictions);
    }

    /// <summary>
    /// Extrae el identificador del usuario autenticado desde los claims del token JWT.
    /// El claim "sub" corresponde al Id del usuario registrado en el token.
    /// </summary>
    /// <returns>Id del usuario autenticado.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el claim no existe o es inválido.</exception>
    private int GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(sub) || !int.TryParse(sub, out var userId))
            throw new UnauthorizedAccessException("No se pudo identificar el usuario autenticado.");

        return userId;
    }
}
