using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;

/// <summary>
/// Contrato del repositorio de usuarios.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su nombre de usuario.
    /// </summary>
    /// <param name="userName">Nombre de usuario a buscar.</param>
    /// <returns>El usuario encontrado o null.</returns>
    Task<User?> GetByUserNameAsync(string userName);

    /// <summary>
    /// Obtiene un usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">Correo electrónico a buscar.</param>
    /// <returns>El usuario encontrado o null.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtiene un usuario por su identificador, incluyendo sus roles.
    /// </summary>
    /// <param name="id">Identificador del usuario.</param>
    /// <returns>El usuario encontrado con roles o null.</returns>
    Task<User?> GetByIdWithRolesAsync(int id);

    /// <summary>
    /// Verifica si ya existe un usuario con el nombre de usuario dado.
    /// </summary>
    /// <param name="userName">Nombre de usuario a verificar.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsByUserNameAsync(string userName);

    /// <summary>
    /// Verifica si ya existe un usuario con el correo electrónico dado.
    /// </summary>
    /// <param name="email">Correo electrónico a verificar.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>
    /// Crea un nuevo usuario en la base de datos.
    /// </summary>
    /// <param name="user">Entidad del usuario a crear.</param>
    /// <returns>El usuario creado con su Id asignado.</returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Actualiza la fecha del último inicio de sesión del usuario.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    Task UpdateLastLoginAsync(int userId);
}
