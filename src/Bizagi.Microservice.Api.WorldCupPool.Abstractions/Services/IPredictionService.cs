using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Prediction;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de predicciones.
/// </summary>
public interface IPredictionService
{
    /// <summary>
    /// Crea una nueva predicción o actualiza la existente del usuario autenticado para el partido dado.
    /// No se permite modificar predicciones de partidos finalizados.
    /// </summary>
    /// <param name="dto">Datos de la predicción.</param>
    /// <param name="userId">Identificador del usuario autenticado obtenido desde el JWT.</param>
    /// <returns>Predicción creada o actualizada.</returns>
    Task<PredictionDto> CreateOrUpdateAsync(PredictionCreateDto dto, int userId);

    /// <summary>
    /// Obtiene todas las predicciones activas del usuario autenticado.
    /// </summary>
    /// <param name="userId">Identificador del usuario autenticado obtenido desde el JWT.</param>
    /// <returns>Lista de predicciones del usuario.</returns>
    Task<IEnumerable<PredictionDto>> GetMineAsync(int userId);
}
