namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa la relación muchos a muchos entre usuarios y roles.
/// </summary>
public class UserRole : BaseEntity
{
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public int IdUser { get; set; }

    /// <summary>
    /// Identificador del rol.
    /// </summary>
    public int IdRole { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el usuario.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Propiedad de navegación hacia el rol.
    /// </summary>
    public Role Role { get; set; } = null!;
}
