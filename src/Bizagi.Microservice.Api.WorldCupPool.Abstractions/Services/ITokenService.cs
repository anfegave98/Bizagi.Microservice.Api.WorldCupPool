using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de generación y validación de tokens JWT.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Genera un token JWT para el usuario dado, incluyendo sus roles como claims.
    /// </summary>
    /// <param name="user">Usuario para el cual generar el token.</param>
    /// <param name="roles">Lista de nombres de roles del usuario.</param>
    /// <returns>Token JWT firmado.</returns>
    string GenerateToken(User user, IEnumerable<string> roles);

    /// <summary>
    /// Retorna el tiempo de expiración del token en segundos, según la configuración.
    /// </summary>
    int GetExpirationSeconds();
}
