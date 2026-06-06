namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;

/// <summary>
/// Opciones tipadas para la configuración de Swagger leídas desde appsettings.json.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// Sección de configuración en appsettings.json.
    /// </summary>
    public const string SectionName = "Swagger";

    /// <summary>
    /// Indica si Swagger está habilitado. Solo debe activarse en ambientes de desarrollo.
    /// </summary>
    public bool Enabled { get; set; } = false;
}
