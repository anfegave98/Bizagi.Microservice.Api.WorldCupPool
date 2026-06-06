using System.ComponentModel.DataAnnotations;

namespace Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

/// <summary>
/// DTO para registrar el resultado real de un partido por parte del administrador.
/// </summary>
public class MatchResultCreateDto
{
    /// <summary>
    /// Goles reales del equipo local. Debe ser mayor o igual a cero.
    /// </summary>
    [Required(ErrorMessage = "Los goles del equipo local son requeridos.")]
    [Range(0, int.MaxValue, ErrorMessage = "Los goles del equipo local deben ser mayores o iguales a cero.")]
    public int HomeGoals { get; set; }

    /// <summary>
    /// Goles reales del equipo visitante. Debe ser mayor o igual a cero.
    /// </summary>
    [Required(ErrorMessage = "Los goles del equipo visitante son requeridos.")]
    [Range(0, int.MaxValue, ErrorMessage = "Los goles del equipo visitante deben ser mayores o iguales a cero.")]
    public int AwayGoals { get; set; }
}
