namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa la predicción de un usuario para un partido.
/// </summary>
public class Prediction : BaseEntity
{
    /// <summary>
    /// Identificador del usuario que realizó la predicción.
    /// </summary>
    public int IdUser { get; set; }

    /// <summary>
    /// Identificador del partido al que corresponde la predicción.
    /// </summary>
    public int IdMatch { get; set; }

    /// <summary>
    /// Goles predichos para el equipo local.
    /// </summary>
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles predichos para el equipo visitante.
    /// </summary>
    public int AwayGoals { get; set; }

    /// <summary>
    /// Puntos obtenidos por esta predicción (se actualiza tras el cálculo).
    /// </summary>
    public int Points { get; set; } = 0;

    /// <summary>
    /// Indica si los puntos ya fueron calculados.
    /// </summary>
    public bool IsCalculated { get; set; } = false;

    /// <summary>
    /// Fecha y hora en que se calcularon los puntos.
    /// </summary>
    public DateTime? CalculatedDate { get; set; }

    /// <summary>
    /// Identificador del usuario que creó el registro.
    /// </summary>
    public int IdUserCreator { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro.
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el usuario.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Propiedad de navegación hacia el partido.
    /// </summary>
    public Match Match { get; set; } = null!;

    /// <summary>
    /// Registros de trazabilidad del cálculo de puntaje.
    /// </summary>
    public ICollection<ScoreLog> ScoreLogs { get; set; } = new List<ScoreLog>();
}
