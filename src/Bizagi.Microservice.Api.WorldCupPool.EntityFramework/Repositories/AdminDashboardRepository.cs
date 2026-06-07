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
        var totalUsers = await _context.Users
        .CountAsync(u => u.State && u.IsActive);

        var totalMatches = await _context.Matches
            .CountAsync(m => m.State);

        var finishedMatches = await _context.Matches
            .CountAsync(m => m.State && m.Status == MatchStatus.Finished);

        var pendingMatches = await _context.Matches
            .CountAsync(m => m.State && m.Status == MatchStatus.Scheduled);

        var totalPredictions = await _context.Predictions
            .CountAsync(p => p.State);

        var calculatedPredictions = await _context.Predictions
            .CountAsync(p => p.State && p.IsCalculated);

        return new AdminDashboardDto
        {
            TotalUsers = totalUsers,
            TotalMatches = totalMatches,
            FinishedMatches = finishedMatches,
            PendingMatches = pendingMatches,
            TotalPredictions = totalPredictions,
            CalculatedPredictions = calculatedPredictions
        };
    }
}
