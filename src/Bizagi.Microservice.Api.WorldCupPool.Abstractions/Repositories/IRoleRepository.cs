using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de roles.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Obtiene un rol por su nombre.
    /// </summary>
    /// <param name="name">Nombre del rol (e.g. "User", "Admin").</param>
    /// <returns>El rol encontrado o null.</returns>
    Task<Role?> GetByNameAsync(string name);
}
