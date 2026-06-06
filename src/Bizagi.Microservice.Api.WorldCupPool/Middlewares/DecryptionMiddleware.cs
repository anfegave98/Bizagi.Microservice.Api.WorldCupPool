using System.Text;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de descifrado de requests para los endpoints críticos.
/// Intercepta solicitudes POST con body cifrado en formato <c>{ "data": "&lt;Base64 AES&gt;" }</c>,
/// descifra el payload y reemplaza el stream de lectura antes de que llegue al controller.
///
/// Endpoints críticos cubiertos:
/// <list type="bullet">
///   <item>POST api/v1/auth/register</item>
///   <item>POST api/v1/auth/login</item>
///   <item>POST api/v1/predictions</item>
///   <item>POST api/v1/admin/matches/{id}/result</item>
/// </list>
///
/// Cuando <see cref="EncryptionOptions.Enabled"/> es false (desarrollo),
/// el middleware es completamente transparente.
/// </summary>
public class DecryptionMiddleware
{
    private readonly RequestDelegate    _next;
    private readonly EncryptionOptions  _options;
    private readonly ILogger<DecryptionMiddleware> _logger;

    /// <summary>
    /// Rutas críticas donde se aplica el descifrado.
    /// Se evalúan como prefijos para cubrir variantes como /api/v1/admin/matches/1/result.
    /// </summary>
    private static readonly string[] _encryptedRoutes =
    {
        "/api/v1/auth/register",
        "/api/v1/auth/login",
        "/api/v1/predictions",
        "/api/v1/admin/matches",
    };

    /// <summary>
    /// Inicializa una nueva instancia del middleware de descifrado.
    /// </summary>
    /// <param name="next">Siguiente delegado en el pipeline.</param>
    /// <param name="options">Opciones tipadas de configuración de cifrado.</param>
    /// <param name="logger">Logger del middleware.</param>
    public DecryptionMiddleware(
        RequestDelegate next,
        IOptions<EncryptionOptions> options,
        ILogger<DecryptionMiddleware> logger)
    {
        _next              = next;
        _options           = options.Value;
        _logger            = logger;
    }

    /// <summary>
    /// Intercepta la solicitud. Si el cifrado está habilitado, la ruta es crítica
    /// y el método es POST, descifra el body y reemplaza el stream.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud.</param>
    /// <param name="encryptionService">Servicio de cifrado inyectado para realizar el descifrado.</param>
    public async Task InvokeAsync(
    HttpContext context,
    IEncryptionService encryptionService)
    {
        if (!_options.Enabled
            || !HttpMethods.IsPost(context.Request.Method)
            || !IsEncryptedRoute(context.Request.Path)
            || !IsJsonContentType(context.Request.ContentType))
        {
            await _next(context);
            return;
        }

        try
        {
            // Habilitar buffering para poder releer el stream si es necesario
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var rawBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(rawBody))
            {
                await _next(context);
                return;
            }

            // Intentar deserializar como wrapper { "data": "..." }
            var wrapper = TryDeserializeWrapper(rawBody);

            if (wrapper is null || string.IsNullOrWhiteSpace(wrapper.Data))
            {
                // No tiene formato cifrado — puede ser Swagger en desarrollo
                // con Enabled=false o un request mal formado
                _logger.LogWarning(
                    "Request a ruta crítica sin payload cifrado. Path: {Path} Method: {Method}",
                    context.Request.Path, context.Request.Method);

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                    "El payload debe estar cifrado. Formato esperado: { \"data\": \"<Base64>\" }");
                return;
            }

            // Descifrar
            var decryptedJson = encryptionService.Decrypt(wrapper.Data);

            if (string.IsNullOrWhiteSpace(decryptedJson))
            {
                _logger.LogWarning(
                    "Descifrado resultó en payload vacío. Path: {Path}", context.Request.Path);

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                    "El payload cifrado no produjo contenido válido. Verifica la llave y el IV.");
                return;
            }

            // Reemplazar el stream con el JSON descifrado
            var decryptedBytes  = Encoding.UTF8.GetBytes(decryptedJson);
            var decryptedStream = new MemoryStream(decryptedBytes);

            context.Request.Body          = decryptedStream;
            context.Request.ContentLength = decryptedBytes.Length;

            _logger.LogDebug("Payload descifrado correctamente. Path: {Path}", context.Request.Path);

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error inesperado al descifrar el body. Path: {Path}", context.Request.Path);

            await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                "No se pudo procesar el payload cifrado. Verifica la llave y el IV.");
        }
    }

    // ─── Helpers privados ─────────────────────────────────────────────────────

    /// <summary>
    /// Verifica si la ruta de la solicitud corresponde a un endpoint crítico cifrado.
    /// </summary>
    private static bool IsEncryptedRoute(PathString path)
    {
        return _encryptedRoutes.Any(route =>
            path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si el Content-Type es application/json.
    /// </summary>
    private static bool IsJsonContentType(string? contentType)
    {
        return !string.IsNullOrWhiteSpace(contentType)
               && contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Intenta deserializar el body como wrapper cifrado <c>{ "data": "..." }</c>.
    /// </summary>
    private static EncryptedWrapper? TryDeserializeWrapper(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<EncryptedWrapper>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Escribe una respuesta de error JSON estandarizada.
    /// </summary>
    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode  = statusCode;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(
            new { status = statusCode, message },
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(body);
    }

    /// <summary>Modelo interno del wrapper cifrado recibido del cliente.</summary>
    private sealed class EncryptedWrapper
    {
        public string? Data { get; set; }
    }
}
