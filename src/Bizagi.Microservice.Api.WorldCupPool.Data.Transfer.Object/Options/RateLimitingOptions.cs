namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;

/// <summary>
/// Opciones tipadas para la configuración de rate limiting,
/// leídas desde appsettings.json bajo la sección "RateLimiting".
/// Permite ajustar límites por ambiente sin recompilar la aplicación.
/// </summary>
public class RateLimitingOptions
{
    /// <summary>
    /// Nombre de la sección en appsettings.json.
    /// </summary>
    public const string SectionName = "RateLimiting";

    /// <summary>
    /// Indica si el rate limiting está habilitado globalmente.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Límite de solicitudes por ventana de tiempo para la política global por defecto.
    /// </summary>
    public int PermitLimit { get; set; } = 100;

    /// <summary>
    /// Duración de la ventana de tiempo en segundos para la política global.
    /// </summary>
    public int WindowSeconds { get; set; } = 60;

    /// <summary>
    /// Cantidad de solicitudes que pueden encolarse cuando se supera el límite.
    /// 0 = sin cola (rechaza inmediatamente).
    /// </summary>
    public int QueueLimit { get; set; } = 0;

    /// <summary>
    /// Cuando es true, el límite se aplica por usuario autenticado (claim sub del JWT).
    /// </summary>
    public bool ApplyByAuthenticatedUser { get; set; } = true;

    /// <summary>
    /// Cuando es true, el límite se aplica por dirección IP para solicitudes anónimas.
    /// </summary>
    public bool ApplyByIpForAnonymous { get; set; } = true;

    /// <summary>
    /// Código HTTP retornado cuando se supera el límite configurado.
    /// </summary>
    public int RejectedStatusCode { get; set; } = 429;

    /// <summary>
    /// Políticas específicas por tipo de endpoint. Sobreescriben los valores globales.
    /// </summary>
    public RateLimitingPolicies Policies { get; set; } = new();
}

/// <summary>
/// Colección de políticas de rate limiting por categoría de endpoint.
/// </summary>
public class RateLimitingPolicies
{
    /// <summary>
    /// Política para endpoints de autenticación (register, login).
    /// Debe ser más restrictiva que las políticas internas para reducir ataques de fuerza bruta.
    /// </summary>
    public RateLimitingPolicy AuthEndpoints { get; set; } = new() { PermitLimit = 10, WindowSeconds = 60 };

    /// <summary>
    /// Política para endpoints de predicciones de usuarios autenticados.
    /// </summary>
    public RateLimitingPolicy PredictionEndpoints { get; set; } = new() { PermitLimit = 30, WindowSeconds = 60 };

    /// <summary>
    /// Política para endpoints administrativos (resultados, dashboard).
    /// </summary>
    public RateLimitingPolicy AdminEndpoints { get; set; } = new() { PermitLimit = 60, WindowSeconds = 60 };

    /// <summary>
    /// Política para endpoints autenticados de uso general (partidos, leaderboard).
    /// </summary>
    public RateLimitingPolicy DefaultAuthenticated { get; set; } = new() { PermitLimit = 100, WindowSeconds = 60 };

    /// <summary>
    /// Política para endpoints públicos sin autenticación (health check).
    /// </summary>
    public RateLimitingPolicy PublicEndpoints { get; set; } = new() { PermitLimit = 30, WindowSeconds = 60 };
}

/// <summary>
/// Configuración de una política individual de rate limiting.
/// </summary>
public class RateLimitingPolicy
{
    /// <summary>
    /// Número máximo de solicitudes permitidas en la ventana de tiempo.
    /// </summary>
    public int PermitLimit { get; set; } = 100;

    /// <summary>
    /// Duración de la ventana de tiempo en segundos.
    /// </summary>
    public int WindowSeconds { get; set; } = 60;
}
