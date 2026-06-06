namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;

/// <summary>
/// DTO con la información del usuario autenticado, retornado tras registro o login.
/// No expone datos sensibles como PasswordHash ni PasswordSalt.
/// </summary>
public class AuthUserDto
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Roles asignados al usuario.
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Indica si el usuario está activo.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Fecha de creación del usuario.
    /// </summary>
    public DateTime DateCreated { get; set; }
}
