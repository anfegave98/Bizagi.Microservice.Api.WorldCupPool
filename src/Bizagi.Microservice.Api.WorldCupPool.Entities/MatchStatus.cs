namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Constantes que representan los estados posibles de un partido.
/// </summary>
public static class MatchStatus
{
    /// <summary>
    /// El partido está programado y aún no ha sido jugado.
    /// </summary>
    public const string Scheduled = "Scheduled";

    /// <summary>
    /// El partido ha finalizado y se ha registrado el resultado real.
    /// </summary>
    public const string Finished = "Finished";
}
