namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;

/// <summary>
/// Opciones tipadas para la configuración de CORS leídas desde appsettings.json.
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// Sección de configuración en appsettings.json.
    /// </summary>
    public const string SectionName = "Cors";

    /// <summary>
    /// Lista de orígenes permitidos para las solicitudes CORS.
    /// </summary>
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}
