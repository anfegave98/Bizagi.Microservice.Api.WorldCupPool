using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de predicciones.
/// </summary>
public interface IPredictionRepository
{
    /// <summary>
    /// Obtiene la predicción activa de un usuario para un partido específico.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="matchId">Identificador del partido.</param>
    /// <returns>La predicción encontrada o null.</returns>
    Task<Prediction?> GetByUserAndMatchAsync(int userId, int matchId);

    /// <summary>
    /// Obtiene todas las predicciones activas de un usuario, incluyendo datos del partido.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <returns>Lista de predicciones del usuario.</returns>
    Task<IEnumerable<Prediction>> GetAllByUserAsync(int userId);

    /// <summary>
    /// Obtiene todas las predicciones activas para un partido específico.
    /// </summary>
    /// <param name="matchId">Identificador del partido.</param>
    /// <returns>Lista de predicciones del partido.</returns>
    Task<IEnumerable<Prediction>> GetAllByMatchAsync(int matchId);

    /// <summary>
    /// Crea una nueva predicción.
    /// </summary>
    /// <param name="prediction">Entidad de predicción a crear.</param>
    /// <returns>La predicción creada con su Id asignado.</returns>
    Task<Prediction> CreateAsync(Prediction prediction);

    /// <summary>
    /// Actualiza una predicción existente.
    /// </summary>
    /// <param name="prediction">Entidad de predicción a actualizar.</param>
    Task UpdateAsync(Prediction prediction);
}
