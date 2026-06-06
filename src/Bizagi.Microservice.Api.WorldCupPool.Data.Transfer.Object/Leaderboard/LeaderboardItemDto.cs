namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Leaderboard;

/// <summary>
/// DTO que representa una fila del ranking global de la Polla Mundialista.
/// Incluye posición, datos del usuario, puntos totales, cantidad de predicciones
/// y cantidad de marcadores exactos acertados.
/// </summary>
public class LeaderboardItemDto
{
    /// <summary>
    /// Posición del usuario en el ranking (1 = primer lugar).
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int IdUser { get; set; }

    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Suma total de puntos calculados del usuario en partidos finalizados.
    /// </summary>
    public int TotalPoints { get; set; }

    /// <summary>
    /// Cantidad total de predicciones calculadas del usuario.
    /// </summary>
    public int PredictionCount { get; set; }

    /// <summary>
    /// Cantidad de predicciones en las que el usuario acertó el marcador exacto.
    /// Se usa como criterio de desempate secundario.
    /// </summary>
    public int ExactScoreCount { get; set; }
}
