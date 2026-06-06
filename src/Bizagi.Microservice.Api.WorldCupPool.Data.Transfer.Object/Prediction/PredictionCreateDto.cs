using System.ComponentModel.DataAnnotations;

namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Prediction;

/// <summary>
/// DTO para registrar o actualizar la predicción de un usuario para un partido.
/// El IdUser se obtiene desde el JWT; no se recibe desde el cliente.
/// </summary>
public class PredictionCreateDto
{
    /// <summary>
    /// Identificador del partido a predecir.
    /// </summary>
    [Required(ErrorMessage = "El identificador del partido es requerido.")]
    public int IdMatch { get; set; }

    /// <summary>
    /// Goles predichos para el equipo local. Debe ser mayor o igual a cero.
    /// </summary>
    [Required(ErrorMessage = "Los goles del equipo local son requeridos.")]
    [Range(0, int.MaxValue, ErrorMessage = "Los goles del equipo local deben ser mayores o iguales a cero.")]
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles predichos para el equipo visitante. Debe ser mayor o igual a cero.
    /// </summary>
    [Required(ErrorMessage = "Los goles del equipo visitante son requeridos.")]
    [Range(0, int.MaxValue, ErrorMessage = "Los goles del equipo visitante deben ser mayores o iguales a cero.")]
    public int AwayGoals { get; set; }
}
