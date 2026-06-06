using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Match;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de partidos.
/// </summary>
public interface IMatchService
{
    /// <summary>
    /// Obtiene todos los partidos activos ordenados por fecha ascendente.
    /// </summary>
    /// <returns>Lista de partidos disponibles.</returns>
    Task<IEnumerable<MatchDto>> GetAllAsync();
}
