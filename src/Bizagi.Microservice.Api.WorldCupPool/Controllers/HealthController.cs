using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador de health check. Permite verificar la disponibilidad del servicio
/// sin necesidad de autenticación. Útil para monitoreo y load balancers.
/// </summary>
[ApiController]
[Route("api/v1/health")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Verifica que el servicio está disponible y respondiendo correctamente.
    /// </summary>
    /// <returns>Estado del servicio con nombre y timestamp UTC.</returns>
    /// <response code="200">El servicio está disponible y operativo.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAsync()
    {
        return Ok(new
        {
            status    = "Healthy",
            service   = "Bizagi.Microservice.Api.WorldCupPool",
            timestamp = DateTime.UtcNow
        });
    }
}
