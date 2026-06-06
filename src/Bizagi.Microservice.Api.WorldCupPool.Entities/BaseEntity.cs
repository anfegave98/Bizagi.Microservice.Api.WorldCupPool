namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad base con campos de auditoría comunes para todas las entidades del sistema.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Indica si el registro está activo (eliminación lógica).
    /// </summary>
    public bool State { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
