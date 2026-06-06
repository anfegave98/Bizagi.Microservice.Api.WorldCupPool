namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

/// <summary>
/// DTO de respuesta tras registrar el resultado real de un partido.
/// </summary>
public class MatchResultDto
{
    /// <summary>
    /// Identificador único del resultado registrado.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador del partido al que corresponde el resultado.
    /// </summary>
    public int IdMatch { get; set; }

    /// <summary>
    /// Goles reales del equipo local.
    /// </summary>
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo visitante.
    /// </summary>
    public int AwayGoals { get; set; }

    /// <summary>
    /// Identificador del administrador que registró el resultado.
    /// </summary>
    public int RegisteredByUserId { get; set; }

    /// <summary>
    /// Fecha y hora en que se registró el resultado.
    /// </summary>
    public DateTime RegisteredDate { get; set; }
}
