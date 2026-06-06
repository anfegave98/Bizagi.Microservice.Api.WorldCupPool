using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de resultados reales de partidos.
/// </summary>
public interface IMatchResultRepository
{
    /// <summary>
    /// Verifica si ya existe un resultado activo para el partido dado.
    /// </summary>
    /// <param name="matchId">Identificador del partido.</param>
    /// <returns>True si existe un resultado activo, false en caso contrario.</returns>
    Task<bool> ExistsByMatchIdAsync(int matchId);

    /// <summary>
    /// Crea un nuevo resultado de partido.
    /// </summary>
    /// <param name="matchResult">Entidad del resultado a crear.</param>
    /// <returns>El resultado creado con su Id asignado.</returns>
    Task<MatchResult> CreateAsync(MatchResult matchResult);
}
