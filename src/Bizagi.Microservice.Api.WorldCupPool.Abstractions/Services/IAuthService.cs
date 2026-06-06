using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de autenticación y registro de usuarios.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registra un nuevo usuario en el sistema con rol "User" por defecto.
    /// </summary>
    /// <param name="dto">Datos del usuario a registrar.</param>
    /// <returns>Información del usuario creado.</returns>
    Task<AuthUserDto> RegisterAsync(RegisterUserDto dto);

    /// <summary>
    /// Autentica un usuario con sus credenciales y retorna el token JWT junto a los datos de sesión.
    /// </summary>
    /// <param name="dto">Credenciales del usuario.</param>
    /// <returns>Token JWT y datos básicos del usuario autenticado.</returns>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}
