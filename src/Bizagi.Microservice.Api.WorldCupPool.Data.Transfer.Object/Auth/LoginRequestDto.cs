using System.ComponentModel.DataAnnotations;

namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;

/// <summary>
/// DTO para la solicitud de autenticación de un usuario.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Nombre de usuario o correo electrónico del usuario.
    /// </summary>
    [Required(ErrorMessage = "El nombre de usuario o correo electrónico es requerido.")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es requerida.")]
    public string Password { get; set; } = string.Empty;
}
