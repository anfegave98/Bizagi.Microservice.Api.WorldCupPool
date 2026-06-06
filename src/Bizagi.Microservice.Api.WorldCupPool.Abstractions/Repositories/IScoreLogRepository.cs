using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de trazabilidad de cálculo de puntajes.
/// </summary>
public interface IScoreLogRepository
{
    /// <summary>
    /// Crea un registro de trazabilidad de cálculo de puntaje.
    /// </summary>
    /// <param name="scoreLog">Entidad del log a crear.</param>
    Task CreateAsync(ScoreLog scoreLog);
}
