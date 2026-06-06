using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de partidos.
/// </summary>
public interface IMatchRepository
{
    /// <summary>
    /// Obtiene todos los partidos activos ordenados por fecha ascendente, incluyendo grupo.
    /// </summary>
    /// <returns>Lista de partidos activos.</returns>
    Task<IEnumerable<Match>> GetAllActiveAsync();

    /// <summary>
    /// Obtiene un partido por su identificador.
    /// </summary>
    /// <param name="matchId">Identificador del partido.</param>
    /// <returns>El partido encontrado o null.</returns>
    Task<Match?> GetByIdAsync(int matchId);

    /// <summary>
    /// Actualiza el estado de un partido.
    /// </summary>
    /// <param name="matchId">Identificador del partido.</param>
    /// <param name="status">Nuevo estado del partido.</param>
    Task UpdateStatusAsync(int matchId, string status);
}
