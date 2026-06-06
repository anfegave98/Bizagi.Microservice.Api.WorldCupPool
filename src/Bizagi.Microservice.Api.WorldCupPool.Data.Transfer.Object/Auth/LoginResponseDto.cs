namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;

/// <summary>
/// DTO de respuesta al autenticar un usuario correctamente. Incluye el token JWT y datos básicos de sesión.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Token JWT de acceso.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de token (siempre "Bearer").
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Tiempo de expiración del token en segundos.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Información básica del usuario autenticado.
    /// </summary>
    public AuthUserDto User { get; set; } = null!;
}
