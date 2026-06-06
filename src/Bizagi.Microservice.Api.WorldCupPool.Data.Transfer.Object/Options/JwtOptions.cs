namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;

/// <summary>
/// Opciones tipadas para la configuración de JWT leídas desde appsettings.json.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Sección de configuración en appsettings.json.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Emisor del token JWT.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiencia del token JWT.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Clave secreta para firmar el token JWT.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Tiempo de expiración del token en minutos.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}
