using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador del dashboard administrativo. Expone los indicadores operativos del sistema.
/// Solo accesible por usuarios con rol "Admin".
/// </summary>
[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : ControllerBase
{
    private readonly IAdminDashboardService _adminDashboardService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de dashboard administrativo.
    /// </summary>
    /// <param name="adminDashboardService">Servicio de indicadores administrativos.</param>
    public AdminDashboardController(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    /// <summary>
    /// Obtiene los indicadores operativos del sistema: usuarios, partidos, predicciones y estado de cálculo.
    /// Solo accesible por el rol "Admin".
    /// </summary>
    /// <returns>DTO con los indicadores del dashboard administrativo.</returns>
    /// <response code="200">Indicadores retornados exitosamente.</response>
    /// <response code="401">El usuario no está autenticado.</response>
    /// <response code="403">El usuario no tiene el rol Admin.</response>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(AdminDashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDashboardAsync()
    {
        var dashboard = await _adminDashboardService.GetDashboardAsync();
        return Ok(dashboard);
    }
}
