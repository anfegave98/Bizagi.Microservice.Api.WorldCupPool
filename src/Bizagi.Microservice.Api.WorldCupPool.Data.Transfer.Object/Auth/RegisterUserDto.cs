using System.ComponentModel.DataAnnotations;

namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;

/// <summary>
/// DTO para el registro de un nuevo usuario en el sistema.
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// Nombre de usuario único. Máximo 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    [MaxLength(100, ErrorMessage = "El nombre de usuario no puede superar los 100 caracteres.")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario. Máximo 150 caracteres.
    /// </summary>
    [Required(ErrorMessage = "El nombre completo es requerido.")]
    [MaxLength(150, ErrorMessage = "El nombre completo no puede superar los 150 caracteres.")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico único del usuario. Máximo 150 caracteres.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    [MaxLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario. Mínimo 8 caracteres.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es requerida.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; } = string.Empty;
}
