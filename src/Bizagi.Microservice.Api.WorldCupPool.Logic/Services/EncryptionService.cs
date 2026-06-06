using System.Security.Cryptography;
using System.Text;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de cifrado y descifrado usando AES (CBC, PKCS7).
/// La llave, el IV y el estado de habilitación se leen desde <see cref="EncryptionOptions"/>.
/// Cuando <see cref="EncryptionOptions.Enabled"/> es false, los métodos actúan
/// como passthrough sin transformar el valor, lo que facilita el desarrollo local.
/// Las llaves y vectores en Base64 se decodifican en cada operación para garantizar
/// que los cambios de configuración se reflejen sin reiniciar el servicio.
/// </summary>
public class EncryptionService : IEncryptionService
{
    private readonly EncryptionOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de cifrado.
    /// </summary>
    /// <param name="options">Opciones tipadas de configuración de cifrado.</param>
    public EncryptionService(IOptions<EncryptionOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public string Encrypt(string plainText)
    {
        if (!_options.Enabled)
            return plainText;

        ArgumentException.ThrowIfNullOrWhiteSpace(plainText, nameof(plainText));

        var key = Convert.FromBase64String(_options.Key);
        var iv  = Convert.FromBase64String(_options.IV);

        using var aes = Aes.Create();
        aes.Key     = key;
        aes.IV      = iv;
        aes.Mode    = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes  = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(cipherBytes);
    }

    /// <inheritdoc />
    public string Decrypt(string cipherText)
    {
        if (!_options.Enabled)
            return cipherText;

        ArgumentException.ThrowIfNullOrWhiteSpace(cipherText, nameof(cipherText));

        var key         = Convert.FromBase64String(_options.Key);
        var iv          = Convert.FromBase64String(_options.IV);
        var cipherBytes = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key     = key;
        aes.IV      = iv;
        aes.Mode    = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
