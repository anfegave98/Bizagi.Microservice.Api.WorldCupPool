namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa un rol de autorización en el sistema.
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Nombre del rol (e.g. "Admin", "User").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción opcional del rol.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Relación de usuarios asociados a este rol.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
