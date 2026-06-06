namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que registra la trazabilidad del cálculo de puntos por predicción.
/// </summary>
public class ScoreLog : BaseEntity
{
    /// <summary>
    /// Identificador de la predicción evaluada.
    /// </summary>
    public int IdPrediction { get; set; }

    /// <summary>
    /// Identificador del resultado real utilizado para el cálculo.
    /// </summary>
    public int IdMatchResult { get; set; }

    /// <summary>
    /// Goles locales predichos por el usuario.
    /// </summary>
    public int PredictedHomeGoals { get; set; }

    /// <summary>
    /// Goles visitantes predichos por el usuario.
    /// </summary>
    public int PredictedAwayGoals { get; set; }

    /// <summary>
    /// Goles locales reales del partido.
    /// </summary>
    public int RealHomeGoals { get; set; }

    /// <summary>
    /// Goles visitantes reales del partido.
    /// </summary>
    public int RealAwayGoals { get; set; }

    /// <summary>
    /// Puntos asignados como resultado del cálculo.
    /// </summary>
    public int PointsAssigned { get; set; }

    /// <summary>
    /// Regla aplicada: "ExactScore", "WinnerOrDraw" o "Failed".
    /// </summary>
    public string RuleApplied { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora del cálculo.
    /// </summary>
    public DateTime CalculationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Propiedad de navegación hacia la predicción.
    /// </summary>
    public Prediction Prediction { get; set; } = null!;

    /// <summary>
    /// Propiedad de navegación hacia el resultado del partido.
    /// </summary>
    public MatchResult MatchResult { get; set; } = null!;
}
