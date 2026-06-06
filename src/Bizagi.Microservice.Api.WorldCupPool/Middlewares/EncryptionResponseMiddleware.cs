using System.Text;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de cifrado de respuestas para los endpoints críticos.
/// Intercepta la respuesta JSON generada por el controller, cifra el body
/// con AES-256-CBC y lo reemplaza con el wrapper <c>{ "data": "&lt;Base64&gt;" }</c>
/// antes de enviarlo al cliente.
///
/// Endpoints cubiertos:
/// <list type="bullet">
///   <item>POST api/v1/auth/register</item>
///   <item>POST api/v1/auth/login</item>
///   <item>POST api/v1/predictions</item>
///   <item>POST api/v1/admin/matches/{id}/result</item>
/// </list>
///
/// Cuando <see cref="EncryptionOptions.Enabled"/> es false (desarrollo),
/// el middleware es completamente transparente y la respuesta viaja en texto plano,
/// lo que permite usar Swagger con normalidad.
///
/// Orden en el pipeline: debe ir inmediatamente después de
/// <see cref="GlobalExceptionMiddleware"/> y antes de <see cref="DecryptionMiddleware"/>,
/// ya que necesita envolver el stream de respuesta antes de que el resto del
/// pipeline lo escriba.
/// </summary>
public class EncryptionResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly EncryptionOptions _options;
    private readonly ILogger<EncryptionResponseMiddleware> _logger;

    /// <summary>
    /// Rutas críticas cuyas respuestas deben cifrarse.
    /// Se evalúan como prefijos para cubrir variantes dinámicas.
    /// </summary>
    private static readonly string[] _encryptedRoutes =
    {
        "/api/v1/auth/register",
        "/api/v1/auth/login",
        "/api/v1/predictions",
        "/api/v1/admin/matches",
    };

    /// <summary>
    /// Inicializa una nueva instancia del middleware de cifrado de respuestas.
    /// </summary>
    /// <param name="next">Siguiente delegado en el pipeline.</param>
    /// <param name="options">Opciones tipadas de configuración de cifrado.</param>
    /// <param name="logger">Logger del middleware.</param>
    public EncryptionResponseMiddleware(
        RequestDelegate next,
        IOptions<EncryptionOptions> options,
        ILogger<EncryptionResponseMiddleware> logger)
    {
        _next = next;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Intercepta la solicitud, ejecuta el pipeline completo capturando la respuesta
    /// en un buffer, la cifra si corresponde y la escribe en el stream original.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud.</param>
    /// <param name="encryptionService">Servicio de cifrado inyectado.</param>
    public async Task InvokeAsync(
    HttpContext context,
    IEncryptionService encryptionService)
    {
        // Si el cifrado está deshabilitado o la ruta no es crítica, pasar sin modificar
        if (!_options.Enabled
            || !HttpMethods.IsPost(context.Request.Method)
            || !IsEncryptedRoute(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Sustituir el stream de respuesta por un buffer para capturar lo que escriba el controller
        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            // Ejecutar el resto del pipeline (controller escribe en el buffer)
            await _next(context);

            // Leer el body generado por el controller
            buffer.Position = 0;
            using var reader = new StreamReader(buffer, Encoding.UTF8);
            var responseBody = await reader.ReadToEndAsync();

            // Solo cifrar respuestas JSON exitosas (2xx)
            if (!IsSuccessStatusCode(context.Response.StatusCode)
                || string.IsNullOrWhiteSpace(responseBody))
            {
                // Respuesta de error o vacía: devolver sin cifrar
                // (los errores del GlobalExceptionMiddleware ya tienen su formato)
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
                return;
            }

            // Cifrar el body JSON
            var encrypted = encryptionService.Encrypt(responseBody);
            var wrappedJson = JsonSerializer.Serialize(
                new { data = encrypted },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var wrappedBytes = Encoding.UTF8.GetBytes(wrappedJson);

            // Actualizar Content-Length y escribir en el stream original
            context.Response.ContentLength = wrappedBytes.Length;
            context.Response.Body = originalBody;
            await originalBody.WriteAsync(wrappedBytes);

            _logger.LogDebug(
                "Respuesta cifrada correctamente. Path: {Path} StatusCode: {Status}",
                context.Request.Path, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al cifrar la respuesta. Path: {Path}", context.Request.Path);

            // Restaurar stream original y dejar pasar la respuesta sin cifrar
            // para no romper el flujo en caso de error inesperado en el cifrado
            context.Response.Body = originalBody;
            buffer.Position = 0;
            await buffer.CopyToAsync(originalBody);
        }
    }

    // ─── Helpers privados ─────────────────────────────────────────────────────

    /// <summary>
    /// Verifica si la ruta corresponde a un endpoint crítico.
    /// </summary>
    private static bool IsEncryptedRoute(PathString path)
    {
        return _encryptedRoutes.Any(route =>
            path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si el código de estado HTTP es exitoso (200–299).
    /// </summary>
    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode is >= 200 and <= 299;
    }
}
