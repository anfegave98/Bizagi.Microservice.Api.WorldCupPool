using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

/// <summary>
/// Contrato del servicio de dashboard administrativo.
/// </summary>
public interface IAdminDashboardService
{
    /// <summary>
    /// Obtiene los indicadores operativos del sistema para el panel administrativo.
    /// Solo accesible por usuarios con rol "Admin".
    /// </summary>
    /// <returns>DTO con los indicadores del dashboard.</returns>
    Task<AdminDashboardDto> GetDashboardAsync();
}
