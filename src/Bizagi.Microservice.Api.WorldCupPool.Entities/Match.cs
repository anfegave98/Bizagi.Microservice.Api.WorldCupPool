namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa un partido precargado del mundial.
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// Identificador del grupo al que pertenece el partido.
    /// </summary>
    public int IdGroup { get; set; }

    /// <summary>
    /// Identificador del equipo local.
    /// </summary>
    public int IdHomeTeam { get; set; }

    /// <summary>
    /// Identificador del equipo visitante.
    /// </summary>
    public int IdAwayTeam { get; set; }

    /// <summary>
    /// Fecha y hora del partido.
    /// </summary>
    public DateTime MatchDate { get; set; }

    /// <summary>
    /// Estado del partido: "Scheduled" o "Finished".
    /// </summary>
    public string Status { get; set; } = MatchStatus.Scheduled;

    /// <summary>
    /// Nombre de la ronda o jornada (e.g. "Fase de grupos").
    /// </summary>
    public string RoundName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del equipo local (desnormalizado para consultas rápidas).
    /// </summary>
    public string HomeTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del equipo visitante (desnormalizado para consultas rápidas).
    /// </summary>
    public string AwayTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de última modificación del registro.
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el grupo.
    /// </summary>
    public Group Group { get; set; } = null!;

    /// <summary>
    /// Propiedad de navegación hacia el equipo local.
    /// </summary>
    public Team HomeTeam { get; set; } = null!;

    /// <summary>
    /// Propiedad de navegación hacia el equipo visitante.
    /// </summary>
    public Team AwayTeam { get; set; } = null!;

    /// <summary>
    /// Predicciones registradas para este partido.
    /// </summary>
    public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();

    /// <summary>
    /// Resultado real registrado por el administrador.
    /// </summary>
    public MatchResult? MatchResult { get; set; }
}
