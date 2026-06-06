namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa un usuario registrado en el sistema.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Nombre de usuario único en el sistema.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico único del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash seguro de la contraseña del usuario.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt utilizado para el hash de la contraseña.
    /// </summary>
    public string? PasswordSalt { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el usuario está activo en el sistema.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Fecha y hora del último inicio de sesión.
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Identificador del usuario que creó este registro.
    /// </summary>
    public int IdUserCreator { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro.
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// Relación de roles asignados al usuario.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Predicciones realizadas por el usuario.
    /// </summary>
    public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
}
