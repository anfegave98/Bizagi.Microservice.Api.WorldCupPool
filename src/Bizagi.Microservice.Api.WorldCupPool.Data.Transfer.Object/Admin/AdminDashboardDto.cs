namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

/// <summary>
/// DTO con los indicadores operativos del sistema para el dashboard administrativo.
/// Solo accesible por usuarios con rol "Admin".
/// </summary>
public class AdminDashboardDto
{
    /// <summary>
    /// Total de usuarios registrados y activos en el sistema.
    /// </summary>
    public int TotalUsers { get; set; }

    /// <summary>
    /// Total de partidos cargados en el sistema (activos).
    /// </summary>
    public int TotalMatches { get; set; }

    /// <summary>
    /// Cantidad de partidos con resultado real registrado (estado "Finished").
    /// </summary>
    public int FinishedMatches { get; set; }

    /// <summary>
    /// Cantidad de partidos pendientes de resultado (estado "Scheduled").
    /// </summary>
    public int PendingMatches { get; set; }

    /// <summary>
    /// Total de predicciones activas registradas en el sistema.
    /// </summary>
    public int TotalPredictions { get; set; }

    /// <summary>
    /// Cantidad de predicciones que ya tienen puntos calculados.
    /// </summary>
    public int CalculatedPredictions { get; set; }
}
