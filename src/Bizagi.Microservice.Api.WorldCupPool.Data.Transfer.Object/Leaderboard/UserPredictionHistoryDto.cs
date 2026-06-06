namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;

/// <summary>
/// DTO que representa una entrada del historial de predicciones de un usuario específico.
/// Incluye los datos del partido, la predicción realizada, el resultado real y los puntos obtenidos.
/// </summary>
public class UserPredictionHistoryDto
{
    /// <summary>
    /// Identificador del partido al que corresponde la predicción.
    /// </summary>
    public int IdMatch { get; set; }

    /// <summary>
    /// Nombre del equipo local.
    /// </summary>
    public string HomeTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del equipo visitante.
    /// </summary>
    public string AwayTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Goles predichos para el equipo local por el usuario.
    /// </summary>
    public int PredictedHomeGoals { get; set; }

    /// <summary>
    /// Goles predichos para el equipo visitante por el usuario.
    /// </summary>
    public int PredictedAwayGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo local. Null si el partido aún no ha finalizado.
    /// </summary>
    public int? RealHomeGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo visitante. Null si el partido aún no ha finalizado.
    /// </summary>
    public int? RealAwayGoals { get; set; }

    /// <summary>
    /// Puntos obtenidos por esta predicción (0 si aún no está calculada).
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Regla de puntuación aplicada: "ExactScore", "WinnerOrDraw", "Failed" o vacío si no calculada.
    /// </summary>
    public string RuleApplied { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual del partido asociado: "Scheduled" o "Finished".
    /// </summary>
    public string MatchStatus { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora del partido.
    /// </summary>
    public DateTime MatchDate { get; set; }
}
