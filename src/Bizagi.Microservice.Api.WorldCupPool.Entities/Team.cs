namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa un equipo participante en el mundial.
/// </summary>
public class Team : BaseEntity
{
    /// <summary>
    /// Nombre completo del equipo.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Código corto del equipo (e.g. "COL", "BRA").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// URL de la bandera del equipo.
    /// </summary>
    public string? FlagUrl { get; set; }

    /// <summary>
    /// Identificador del grupo al que pertenece el equipo.
    /// </summary>
    public int IdGroup { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el grupo.
    /// </summary>
    public Group Group { get; set; } = null!;
}
