namespace Bizagi.Microservice.Api.WorldCupPool.Entities;

/// <summary>
/// Entidad que representa un grupo de la fase de grupos del mundial.
/// </summary>
public class Group : BaseEntity
{
    /// <summary>
    /// Nombre del grupo (e.g. "Grupo A", "Grupo B").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción opcional del grupo.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Equipos pertenecientes a este grupo.
    /// </summary>
    public ICollection<Team> Teams { get; set; } = new List<Team>();

    /// <summary>
    /// Partidos asociados a este grupo.
    /// </summary>
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
