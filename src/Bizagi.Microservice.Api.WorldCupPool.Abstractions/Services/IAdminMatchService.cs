using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio administrativo de partidos.
/// </summary>
public interface IAdminMatchService
{
    /// <summary>
    /// Registra el resultado real de un partido. Solo accesible por administradores.
    /// Al registrar el resultado, el partido cambia a estado Finished y se dispara el cálculo de puntos.
    /// </summary>
    /// <param name="matchId">Identificador del partido.</param>
    /// <param name="dto">Goles reales del partido.</param>
    /// <param name="adminUserId">Identificador del administrador autenticado obtenido desde el JWT.</param>
    /// <returns>Resultado registrado.</returns>
    Task<MatchResultDto> RegisterResultAsync(decimal matchId, MatchResultCreateDto dto, decimal adminUserId);
}
