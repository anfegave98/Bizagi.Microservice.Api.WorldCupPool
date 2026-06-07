using System.Text;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de cifrado de respuestas para los endpoints críticos.
/// Intercepta la respuesta JSON del controller, la cifra con AES-256-CBC
/// y la reemplaza con el wrapper <c>{ "data": "&lt;Base64&gt;" }</c>.
///
/// Preserva todos los headers de respuesta existentes (incluyendo CORS)
/// al reemplazar únicamente el body, no el stream completo de respuesta.
///
/// Cuando <see cref="EncryptionOptions.Enabled"/> es false, el middleware
/// es completamente transparente.
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
    /// Inicializa el middleware. Usa <see cref="IServiceScopeFactory"/> en lugar de
    /// <see cref="IEncryptionService"/> directamente para evitar el error
    /// "Cannot resolve scoped service from root provider", ya que los middlewares
    /// son singleton por naturaleza del pipeline de ASP.NET Core.
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
    /// Captura la respuesta del controller en un buffer, la cifra y la reemplaza
    /// preservando todos los headers HTTP originales (incluyendo CORS).
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

        // Sustituir el body de la respuesta por un buffer
        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            // Ejecutar el resto del pipeline — el controller escribe en el buffer
            await _next(context);

            // Leer el body capturado
            buffer.Position = 0;
            using var reader = new StreamReader(buffer, Encoding.UTF8);
            var responseBody = await reader.ReadToEndAsync();

            // Solo cifrar respuestas JSON exitosas (2xx)
            if (!IsSuccessStatusCode(context.Response.StatusCode)
                || string.IsNullOrWhiteSpace(responseBody))
            {
                // Error o body vacío: devolver sin cifrar preservando headers
                buffer.Position = 0;
                context.Response.Body = originalBody;
                await buffer.CopyToAsync(originalBody);
                return;
            }

            // Cifrar usando un scope para resolver el servicio Scoped correctamente
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

            // Actualizar Content-Length — los demás headers (CORS, Content-Type, etc.)
            // ya están escritos en context.Response.Headers y se preservan automáticamente
            context.Response.ContentLength = wrappedBytes.Length;

            // Restaurar el stream original y escribir el body cifrado
            context.Response.Body = originalBody;
            await originalBody.WriteAsync(wrappedBytes);

            _logger.LogDebug(
                "Respuesta cifrada. Path: {Path} StatusCode: {Status}",
                context.Request.Path, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al cifrar la respuesta. Path: {Path}", context.Request.Path);

            // Restaurar stream original en caso de error
            context.Response.Body = originalBody;
            buffer.Position = 0;
            await buffer.CopyToAsync(originalBody);
        }
    }

    private static bool IsEncryptedRoute(PathString path)
        => _encryptedRoutes.Any(route =>
            path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase));

    private static bool IsSuccessStatusCode(int statusCode)
        => statusCode is >= 200 and <= 299;
}
