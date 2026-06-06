namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Match;

/// <summary>
/// DTO que representa un partido disponible para predicción o consulta.
/// </summary>
public class MatchDto
{
    /// <summary>
    /// Identificador único del partido.
    /// </summary>
    public decimal Id { get; set; }

    /// <summary>
    /// Identificador del grupo al que pertenece el partido.
    /// </summary>
    public decimal IdGroup { get; set; }

    /// <summary>
    /// Nombre del grupo (e.g. "Grupo A").
    /// </summary>
    public string GroupName { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del equipo local.
    /// </summary>
    public decimal IdHomeTeam { get; set; }

    /// <summary>
    /// Nombre del equipo local.
    /// </summary>
    public string HomeTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del equipo visitante.
    /// </summary>
    public decimal IdAwayTeam { get; set; }

    /// <summary>
    /// Nombre del equipo visitante.
    /// </summary>
    public string AwayTeamName { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora del partido.
    /// </summary>
    public DateTime MatchDate { get; set; }

    /// <summary>
    /// Nombre de la ronda o jornada.
    /// </summary>
    public string RoundName { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual del partido: "Scheduled" o "Finished".
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
