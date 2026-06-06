using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Admin;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de indicadores administrativos usando Entity Framework Core.
/// Las consultas de conteo se ejecutan de forma independiente para mantener legibilidad
/// y permitir optimización individual por índice.
/// </summary>
public class AdminDashboardRepository : IAdminDashboardRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de dashboard administrativo.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public AdminDashboardRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        // Ejecutar todas las consultas de conteo en paralelo para reducir latencia total.
        var totalUsersTask              = _context.Users.CountAsync(u => u.State && u.IsActive);
        var totalMatchesTask            = _context.Matches.CountAsync(m => m.State);
        var finishedMatchesTask         = _context.Matches.CountAsync(m => m.State && m.Status == MatchStatus.Finished);
        var pendingMatchesTask          = _context.Matches.CountAsync(m => m.State && m.Status == MatchStatus.Scheduled);
        var totalPredictionsTask        = _context.Predictions.CountAsync(p => p.State);
        var calculatedPredictionsTask   = _context.Predictions.CountAsync(p => p.State && p.IsCalculated);

        await Task.WhenAll(
            totalUsersTask,
            totalMatchesTask,
            finishedMatchesTask,
            pendingMatchesTask,
            totalPredictionsTask,
            calculatedPredictionsTask);

        return new AdminDashboardDto
        {
            TotalUsers             = await totalUsersTask,
            TotalMatches           = await totalMatchesTask,
            FinishedMatches        = await finishedMatchesTask,
            PendingMatches         = await pendingMatchesTask,
            TotalPredictions       = await totalPredictionsTask,
            CalculatedPredictions  = await calculatedPredictionsTask
        };
    }
}
