using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de indicadores administrativos.
/// </summary>
public interface IAdminDashboardRepository
{
    /// <summary>
    /// Obtiene los indicadores operativos del sistema para el dashboard administrativo.
    /// Consulta conteos de usuarios, partidos, predicciones y estado de cálculo.
    /// </summary>
    /// <returns>DTO con los indicadores del dashboard.</returns>
    Task<AdminDashboardDto> GetDashboardAsync();
}
