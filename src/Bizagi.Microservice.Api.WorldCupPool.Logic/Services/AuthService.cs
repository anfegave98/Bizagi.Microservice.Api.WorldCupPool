using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Repositories;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;
using Bizagi.Microservice.Api.WorldCupPool.Entities;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de autenticación y registro de usuarios.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de autenticación.
    /// </summary>
    /// <param name="userRepository">Repositorio de usuarios.</param>
    /// <param name="roleRepository">Repositorio de roles.</param>
    /// <param name="passwordService">Servicio de hash de contraseñas.</param>
    /// <param name="tokenService">Servicio de generación de tokens JWT.</param>
    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    /// <inheritdoc />
    public async Task<AuthUserDto> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepository.ExistsByUserNameAsync(dto.UserName))
            throw new InvalidOperationException("El nombre de usuario ya está en uso.");

        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("El correo electrónico ya está registrado.");

        var userRole = await _roleRepository.GetByNameAsync("User")
            ?? throw new InvalidOperationException("El rol 'User' no está configurado en el sistema.");

        _passwordService.CreateHash(dto.Password, out var hash, out var salt);

        var user = new User
        {
            UserName = dto.UserName,
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsActive = true,
            State = true,
            IdUserCreator = 0,
            DateCreated = DateTime.UtcNow,
            UserRoles = new List<UserRole>
            {
                new()
                {
                    IdRole = userRole.Id,
                    State = true,
                    DateCreated = DateTime.UtcNow
                }
            }
        };

        var created = await _userRepository.CreateAsync(user);

        return new AuthUserDto
        {
            Id = created.Id,
            UserName = created.UserName,
            FullName = created.FullName,
            Email = created.Email,
            Roles = new List<string> { "User" },
            IsActive = created.IsActive,
            DateCreated = created.DateCreated
        };
    }

    /// <inheritdoc />
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        // Buscar por nombre de usuario o email
        var user = await _userRepository.GetByUserNameAsync(dto.UserNameOrEmail)
                   ?? await _userRepository.GetByEmailAsync(dto.UserNameOrEmail);

        if (user is null || !_passwordService.VerifyHash(dto.Password, user.PasswordHash, user.PasswordSalt!))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!user.IsActive)
            throw new InvalidOperationException("El usuario se encuentra inactivo.");

        var roles = user.UserRoles
            .Where(ur => ur.State && ur.Role is not null)
            .Select(ur => ur.Role.Name)
            .ToList();

        var token = _tokenService.GenerateToken(user, roles);

        await _userRepository.UpdateLastLoginAsync(user.Id);

        return new LoginResponseDto
        {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresIn = _tokenService.GetExpirationSeconds(),
            User = new AuthUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles,
                IsActive = user.IsActive,
                DateCreated = user.DateCreated
            }
        };
    }
}
