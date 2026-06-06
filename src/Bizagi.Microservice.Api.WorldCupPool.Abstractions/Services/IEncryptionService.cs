namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de cifrado y descifrado de datos sensibles.
/// Utiliza AES con llave e IV configurados desde appsettings.json.
/// Cuando el cifrado está deshabilitado (Encryption:Enabled = false),
/// los métodos actúan como passthrough sin transformar el valor.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Cifra un valor en texto plano usando AES y retorna el resultado en Base64.
    /// Si el cifrado está deshabilitado, retorna el valor original sin transformar.
    /// </summary>
    /// <param name="plainText">Texto plano a cifrar.</param>
    /// <returns>Texto cifrado en Base64, o el valor original si el cifrado está deshabilitado.</returns>
    string Encrypt(string plainText);

    /// <summary>
    /// Descifra un valor en Base64 cifrado con AES y retorna el texto plano.
    /// Si el cifrado está deshabilitado, retorna el valor original sin transformar.
    /// </summary>
    /// <param name="cipherText">Texto cifrado en Base64 a descifrar.</param>
    /// <returns>Texto plano descifrado, o el valor original si el cifrado está deshabilitado.</returns>
    string Decrypt(string cipherText);
}
