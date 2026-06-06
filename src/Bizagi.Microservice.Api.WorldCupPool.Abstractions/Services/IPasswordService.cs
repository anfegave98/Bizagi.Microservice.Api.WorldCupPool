namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de hash y verificación de contraseñas.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Genera el hash y salt de una contraseña en texto plano.
    /// </summary>
    /// <param name="password">Contraseña en texto plano.</param>
    /// <param name="passwordHash">Hash resultante.</param>
    /// <param name="passwordSalt">Salt generado.</param>
    void CreateHash(string password, out string passwordHash, out string passwordSalt);

    /// <summary>
    /// Verifica si una contraseña en texto plano corresponde al hash y salt almacenados.
    /// </summary>
    /// <param name="password">Contraseña en texto plano a verificar.</param>
    /// <param name="passwordHash">Hash almacenado.</param>
    /// <param name="passwordSalt">Salt almacenado.</param>
    /// <returns>True si la contraseña es válida, false en caso contrario.</returns>
    bool VerifyHash(string password, string passwordHash, string passwordSalt);
}
