using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizagi.Microservice.Api.WorldCupPool.Controllers;

/// <summary>
/// Controlador de autenticación. Expone los endpoints de registro e inicio de sesión.
/// Todos los endpoints de este controlador son públicos (no requieren JWT).
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de autenticación.
    /// </summary>
    /// <param name="authService">Servicio de autenticación.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema con rol "User" por defecto.
    /// </summary>
    /// <param name="dto">Datos del nuevo usuario.</param>
    /// <returns>Información del usuario creado con HTTP 201.</returns>
    /// <response code="201">Usuario creado exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos o nombre de usuario/email ya en uso.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthUserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return CreatedAtAction(nameof(Register), result);
    }

    /// <summary>
    /// Autentica un usuario y retorna el token JWT junto con los datos básicos de sesión.
    /// </summary>
    /// <param name="dto">Credenciales del usuario (nombre de usuario o email, más contraseña).</param>
    /// <returns>Token JWT y datos del usuario autenticado.</returns>
    /// <response code="200">Autenticación exitosa.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="401">Credenciales incorrectas.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}
