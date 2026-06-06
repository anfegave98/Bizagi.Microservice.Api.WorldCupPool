namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa el resultado real de un partido, registrado por el administrador.
/// </summary>
public class MatchResult : BaseEntity
{
    /// <summary>
    /// Identificador del partido al que corresponde el resultado.
    /// </summary>
    public decimal IdMatch { get; set; }

    /// <summary>
    /// Goles reales del equipo local.
    /// </summary>
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo visitante.
    /// </summary>
    public int AwayGoals { get; set; }

    /// <summary>
    /// Identificador del usuario administrador que registró el resultado.
    /// </summary>
    public decimal RegisteredByUserId { get; set; }

    /// <summary>
    /// Fecha y hora en que se registró el resultado.
    /// </summary>
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última modificación del registro.
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el partido.
    /// </summary>
    public Match Match { get; set; } = null!;

    /// <summary>
    /// Registros de trazabilidad del cálculo de puntaje generados por este resultado.
    /// </summary>
    public ICollection<ScoreLog> ScoreLogs { get; set; } = new List<ScoreLog>();
}
