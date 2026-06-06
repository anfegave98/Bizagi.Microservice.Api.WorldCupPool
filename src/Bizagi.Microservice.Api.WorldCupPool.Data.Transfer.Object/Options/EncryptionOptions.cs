namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;

/// <summary>
/// Opciones tipadas para la configuración del servicio de cifrado AES,
/// leídas desde appsettings.json bajo la sección "Encryption".
/// La llave y el IV nunca deben versionarse con valores reales;
/// deben inyectarse desde variables de entorno o secret manager en producción.
/// </summary>
public class EncryptionOptions
{
    /// <summary>
    /// Nombre de la sección en appsettings.json.
    /// </summary>
    public const string SectionName = "Encryption";

    /// <summary>
    /// Indica si el cifrado está habilitado. Cuando es false, el servicio actúa
    /// como passthrough (retorna el valor sin transformar), útil en desarrollo.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Algoritmo de cifrado a utilizar. Valor esperado: "AES".
    /// </summary>
    public string Algorithm { get; set; } = "AES";

    /// <summary>
    /// Clave de cifrado en Base64. Debe ser de 16, 24 o 32 bytes (128, 192 o 256 bits).
    /// No versionar con valor real. Leer desde variable de entorno o secret manager.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Vector de inicialización (IV) en Base64. Debe ser de exactamente 16 bytes (128 bits).
    /// No versionar con valor real. Leer desde variable de entorno o secret manager.
    /// </summary>
    public string IV { get; set; } = string.Empty;
}
