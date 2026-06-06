using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizagi.Microservice.Api.WorldCupPool.EntityFramework.Repositories;

/// <summary>
/// Implementación del repositorio de roles usando Entity Framework Core.
/// </summary>
public class RoleRepository : IRoleRepository
{
    private readonly WorldCupPoolDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de roles.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public RoleRepository(WorldCupPoolDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name && r.State);
    }
}
