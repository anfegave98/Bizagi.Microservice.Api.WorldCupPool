namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Constantes que representan las reglas de puntuación aplicadas al calcular un puntaje.
/// </summary>
public static class ScoreRule
{
    /// <summary>
    /// Se acertó el marcador exacto (local y visitante). Otorga 3 puntos.
    /// </summary>
    public const string ExactScore = "ExactScore";

    /// <summary>
    /// Se acertó el ganador del partido o el empate. Otorga 1 punto.
    /// </summary>
    public const string WinnerOrDraw = "WinnerOrDraw";

    /// <summary>
    /// No se acertó ni el marcador ni el ganador. Otorga 0 puntos.
    /// </summary>
    public const string Failed = "Failed";
}
