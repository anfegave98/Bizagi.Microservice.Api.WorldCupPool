using System.Text;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de cifrado de respuestas para los endpoints críticos.
/// Captura el body de la respuesta DESPUÉS de que todo el pipeline ejecutó,
/// lee el status code real y solo cifra si es una respuesta exitosa (2xx).
/// Las respuestas de error (4xx, 5xx) pasan sin cifrar para que el frontend
/// pueda leer el mensaje de error directamente.
/// </summary>
public class EncryptionResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EncryptionOptions _options;
    private readonly ILogger<EncryptionResponseMiddleware> _logger;

    private static readonly string[] _encryptedRoutes =
    {
        "/api/v1/auth/register",
        "/api/v1/auth/login",
        "/api/v1/predictions",
        "/api/v1/admin/matches",
    };

    /// <summary>
    /// Inicializa el middleware usando <see cref="IServiceScopeFactory"/> para evitar
    /// el error "Cannot resolve scoped service from root provider".
    /// </summary>
    public EncryptionResponseMiddleware(
        RequestDelegate next,
        IServiceScopeFactory scopeFactory,
        IOptions<EncryptionOptions> options,
        ILogger<EncryptionResponseMiddleware> logger)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Intercepta la respuesta, ejecuta todo el pipeline incluyendo manejo de errores,
    /// y luego decide si cifrar basándose en el status code resultante.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.Enabled
            || !HttpMethods.IsPost(context.Request.Method)
            || !IsEncryptedRoute(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Reemplazar el stream de respuesta con un buffer para capturar todo
        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            // Ejecutar TODO el pipeline: controllers, GlobalExceptionMiddleware, etc.
            // Al terminar, el buffer tiene el body completo y context.Response.StatusCode
            // refleja el status code REAL de la respuesta (200, 400, 401, etc.)
            await _next(context);

            // Leer el body capturado
            buffer.Position = 0;
            using var reader = new StreamReader(buffer, Encoding.UTF8);
            var responseBody = await reader.ReadToEndAsync();

            // Restaurar el stream original antes de escribir
            context.Response.Body = originalBody;

            // ── Respuestas de error (4xx, 5xx) o body vacío: NO cifrar ───────
            // El frontend necesita leer el mensaje de error en texto plano
            if (!IsSuccessStatusCode(context.Response.StatusCode)
                || string.IsNullOrWhiteSpace(responseBody))
            {
                _logger.LogDebug(
                    "[EncryptionResponse] Status {Status} — devolviendo sin cifrar. Path: {Path}",
                    context.Response.StatusCode, context.Request.Path);

                var errorBytes = Encoding.UTF8.GetBytes(responseBody);
                context.Response.ContentLength = errorBytes.Length;
                await originalBody.WriteAsync(errorBytes);
                return;
            }

            // ── Respuestas exitosas (2xx): cifrar ────────────────────────────
            string encrypted;
            using (var scope = _scopeFactory.CreateScope())
            {
                var encryptionService = scope.ServiceProvider
                    .GetRequiredService<IEncryptionService>();
                encrypted = encryptionService.Encrypt(responseBody);
            }

            var wrappedJson = JsonSerializer.Serialize(
                new { data = encrypted },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var wrappedBytes = Encoding.UTF8.GetBytes(wrappedJson);
            context.Response.ContentLength = wrappedBytes.Length;
            await originalBody.WriteAsync(wrappedBytes);

            _logger.LogDebug(
                "[EncryptionResponse] Respuesta cifrada (status {Status}). Path: {Path}",
                context.Response.StatusCode, context.Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[EncryptionResponse] Error inesperado. Path: {Path}",
                context.Request.Path);

            context.Response.Body = originalBody;

            throw;
        }
    }

    private static bool IsEncryptedRoute(PathString path)
        => _encryptedRoutes.Any(route =>
            path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase));

    private static bool IsSuccessStatusCode(int statusCode)
        => statusCode is >= 200 and <= 299;
}
