using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de dashboard administrativo.
/// </summary>
public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminDashboardRepository _adminDashboardRepository;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de dashboard administrativo.
    /// </summary>
    /// <param name="adminDashboardRepository">Repositorio de indicadores administrativos.</param>
    public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository)
    {
        _adminDashboardRepository = adminDashboardRepository;
    }

    /// <inheritdoc />
    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        return await _adminDashboardRepository.GetDashboardAsync();
    }
}
