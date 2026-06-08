using System.Text;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de descifrado de requests para los endpoints críticos.
/// Intercepta el body cifrado <c>{ "data": "&lt;Base64 AES&gt;" }</c>,
/// lo descifra y reemplaza el stream antes de que llegue al controller.
/// </summary>
public class DecryptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EncryptionOptions _options;
    private readonly ILogger<DecryptionMiddleware> _logger;

    private static readonly string[] _encryptedRoutes =
    {
        "/api/v1/auth/register",
        "/api/v1/auth/login",
        "/api/v1/predictions",
        "/api/v1/admin/matches",
    };

    /// <summary>
    /// Inicializa el middleware usando <see cref="IServiceScopeFactory"/>
    /// para evitar el error "Cannot resolve scoped service from root provider".
    /// </summary>
    public DecryptionMiddleware(
        RequestDelegate next,
        IServiceScopeFactory scopeFactory,
        IOptions<EncryptionOptions> options,
        ILogger<DecryptionMiddleware> logger)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Intercepta el request, descifra el body si es una ruta crítica con cifrado habilitado
    /// y reemplaza el stream con el JSON original antes de pasar al controller.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        // Log diagnóstico para identificar por qué el middleware no intercepta
        _logger.LogDebug(
            "[Decryption] Path: {Path} | Method: {Method} | ContentType: {CT} | Enabled: {Enabled} | IsRoute: {IsRoute}",
            context.Request.Path,
            context.Request.Method,
            context.Request.ContentType ?? "NULL",
            _options.Enabled,
            IsEncryptedRoute(context.Request.Path));

        // Si el cifrado está deshabilitado, pasar sin modificar
        if (!_options.Enabled)
        {
            _logger.LogDebug("[Decryption] Cifrado deshabilitado — pasando sin modificar.");
            await _next(context);
            return;
        }

        // Solo aplica a métodos POST
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await _next(context);
            return;
        }

        // Solo aplica a rutas críticas
        if (!IsEncryptedRoute(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Verificación relajada del Content-Type:
        // Angular puede enviar "application/json", "application/json; charset=utf-8", etc.
        // También acepta null/vacío y lo intenta procesar igual para mayor tolerancia.
        var contentType = context.Request.ContentType ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(contentType)
            && !contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(
                "[Decryption] Content-Type no es JSON: '{CT}' — pasando sin modificar.",
                contentType);
            await _next(context);
            return;
        }

        try
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var rawBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            _logger.LogDebug("[Decryption] Body recibido ({Length} bytes): {Body}",
                rawBody.Length, rawBody.Length > 200 ? rawBody[..200] + "..." : rawBody);

            if (string.IsNullOrWhiteSpace(rawBody))
            {
                _logger.LogWarning("[Decryption] Body vacío — pasando sin modificar.");
                await _next(context);
                return;
            }

            // Intentar deserializar como wrapper { "data": "..." }
            var wrapper = TryDeserializeWrapper(rawBody);

            if (wrapper is null || string.IsNullOrWhiteSpace(wrapper.Data))
            {
                _logger.LogWarning(
                    "[Decryption] Body no tiene formato cifrado {{ \"data\": \"...\" }}. " +
                    "Path: {Path}. Body: {Body}",
                    context.Request.Path,
                    rawBody.Length > 300 ? rawBody[..300] : rawBody);

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                    "El payload debe estar cifrado. Formato esperado: { \"data\": \"<Base64>\" }");
                return;
            }

            // Descifrar usando un scope para resolver el servicio Scoped correctamente
            string decryptedJson;
            using (var scope = _scopeFactory.CreateScope())
            {
                var encryptionService = scope.ServiceProvider
                    .GetRequiredService<IEncryptionService>();
                decryptedJson = encryptionService.Decrypt(wrapper.Data);
            }

            _logger.LogDebug("[Decryption] JSON descifrado: {Json}", decryptedJson);

            if (string.IsNullOrWhiteSpace(decryptedJson))
            {
                _logger.LogWarning("[Decryption] Descifrado resultó vacío. Path: {Path}",
                    context.Request.Path);

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                    "El payload cifrado no produjo contenido válido. Verifica la llave y el IV.");
                return;
            }

            // Reemplazar el stream con el JSON descifrado
            var decryptedBytes = Encoding.UTF8.GetBytes(decryptedJson);
            var decryptedStream = new MemoryStream(decryptedBytes);

            context.Request.Body = decryptedStream;
            context.Request.ContentLength = decryptedBytes.Length;

            _logger.LogInformation("[Decryption] ✓ Payload descifrado correctamente. Path: {Path}",
                context.Request.Path);

            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Decryption] Error al descifrar. Path: {Path}",
                context.Request.Path);

            await WriteErrorAsync(context, StatusCodes.Status400BadRequest,
                "No se pudo procesar el payload cifrado. Verifica la llave y el IV.");
        }

        await _next(context);
    }

    // ─── Helpers ─────────────────────────────────────────────────────────────

    private static bool IsEncryptedRoute(PathString path)
        => _encryptedRoutes.Any(route =>
            path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase));

    private static EncryptedWrapper? TryDeserializeWrapper(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<EncryptedWrapper>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch { return null; }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(
            new { status = statusCode, message },
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(body);
    }

    private sealed class EncryptedWrapper
    {
        public string? Data { get; set; }
    }
}
