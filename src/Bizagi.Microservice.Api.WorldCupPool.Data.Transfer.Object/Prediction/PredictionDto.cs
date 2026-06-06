namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Prediction;

/// <summary>
/// DTO que representa la predicción de un usuario, incluyendo información del partido y puntos calculados.
/// </summary>
public class PredictionDto
{
    /// <summary>
    /// Identificador único de la predicción.
    /// </summary>
    public decimal Id { get; set; }

    /// <summary>
    /// Identificador del usuario propietario de la predicción.
    /// </summary>
    public decimal IdUser { get; set; }

    /// <summary>
    /// Identificador del partido al que corresponde la predicción.
    /// </summary>
    public decimal IdMatch { get; set; }

    /// <summary>
    /// Nombre del equipo local del partido.
    /// </summary>
    public string HomeTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del equipo visitante del partido.
    /// </summary>
    public string AwayTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Goles predichos para el equipo local.
    /// </summary>
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles predichos para el equipo visitante.
    /// </summary>
    public int AwayGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo local (null si el partido no ha finalizado).
    /// </summary>
    public int? RealHomeGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo visitante (null si el partido no ha finalizado).
    /// </summary>
    public int? RealAwayGoals { get; set; }

    /// <summary>
    /// Puntos obtenidos por esta predicción.
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Indica si los puntos ya fueron calculados.
    /// </summary>
    public bool IsCalculated { get; set; }

    /// <summary>
    /// Estado actual del partido asociado.
    /// </summary>
    public string MatchStatus { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de creación de la predicción.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Fecha de última modificación de la predicción.
    /// </summary>
    public DateTime? DateModified { get; set; }
}
