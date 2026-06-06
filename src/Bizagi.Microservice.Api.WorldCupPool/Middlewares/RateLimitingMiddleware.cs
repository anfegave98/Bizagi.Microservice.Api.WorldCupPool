using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Microsoft.Extensions.Options;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware de rate limiting configurable por política de endpoint, usuario autenticado e IP.
/// Implementa el algoritmo de ventana fija (Fixed Window) en memoria.
/// Las políticas se leen desde <see cref="RateLimitingOptions"/> en appsettings.json,
/// permitiendo ajustar límites sin recompilar la aplicación.
/// Cuando <see cref="RateLimitingOptions.Enabled"/> es false, el middleware es transparente.
/// </summary>
public class RateLimitingMiddleware
{
    /// <summary>
    /// Nombres de las políticas de rate limiting disponibles.
    /// </summary>
    public static class PolicyNames
    {
        /// <summary>Política para endpoints de autenticación (register, login).</summary>
        public const string AuthEndpoints         = "AuthEndpoints";

        /// <summary>Política para endpoints de predicciones.</summary>
        public const string PredictionEndpoints   = "PredictionEndpoints";

        /// <summary>Política para endpoints administrativos.</summary>
        public const string AdminEndpoints        = "AdminEndpoints";

        /// <summary>Política para endpoints autenticados de uso general.</summary>
        public const string DefaultAuthenticated  = "DefaultAuthenticated";

        /// <summary>Política para endpoints públicos sin autenticación.</summary>
        public const string PublicEndpoints       = "PublicEndpoints";
    }

    // Diccionario global de contadores: clave = "policyName:partitionKey", valor = (conteo, expiración)
    private static readonly ConcurrentDictionary<string, RateLimitEntry> _counters = new();

    private readonly RequestDelegate        _next;
    private readonly RateLimitingOptions    _options;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    // Mapa de rutas a nombres de política (prefijo de ruta → nombre de política)
    private static readonly Dictionary<string, string> _routePolicyMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "/api/v1/auth",        PolicyNames.AuthEndpoints        },
        { "/api/v1/predictions", PolicyNames.PredictionEndpoints  },
        { "/api/v1/admin",       PolicyNames.AdminEndpoints       },
        { "/api/v1/health",      PolicyNames.PublicEndpoints      },
    };

    /// <summary>
    /// Inicializa una nueva instancia del middleware de rate limiting.
    /// </summary>
    /// <param name="next">Siguiente delegado en el pipeline.</param>
    /// <param name="options">Opciones tipadas de configuración de rate limiting.</param>
    /// <param name="logger">Logger del middleware.</param>
    public RateLimitingMiddleware(
        RequestDelegate next,
        IOptions<RateLimitingOptions> options,
        ILogger<RateLimitingMiddleware> logger)
    {
        _next    = next;
        _options = options.Value;
        _logger  = logger;
    }

    /// <summary>
    /// Intercepta la solicitud HTTP, aplica la política de rate limiting correspondiente
    /// y rechaza con HTTP 429 si se supera el límite configurado.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.Enabled)
        {
            await _next(context);
            return;
        }

        var policy = ResolvePolicy(context.Request.Path);
        var (permitLimit, windowSeconds) = GetPolicyLimits(policy);
        var partitionKey = BuildPartitionKey(context, policy);

        if (!IsAllowed(partitionKey, permitLimit, windowSeconds))
        {
            _logger.LogWarning(
                "Rate limit superado. Política: {Policy} | Clave: {Key} | Límite: {Limit}/{Window}s",
                policy, partitionKey, permitLimit, windowSeconds);

            context.Response.StatusCode  = _options.RejectedStatusCode;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(
                new { status = _options.RejectedStatusCode, message = "Se ha superado el límite de solicitudes. Por favor espere e intente nuevamente." },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(body);
            return;
        }

        await _next(context);
    }

    // ─── Métodos privados ─────────────────────────────────────────────────────

    /// <summary>
    /// Resuelve el nombre de la política a aplicar según el prefijo de la ruta de la solicitud.
    /// Si no coincide con ningún prefijo configurado, aplica la política DefaultAuthenticated.
    /// </summary>
    private static string ResolvePolicy(PathString path)
    {
        foreach (var (prefix, policyName) in _routePolicyMap)
        {
            if (path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase))
                return policyName;
        }
        return PolicyNames.DefaultAuthenticated;
    }

    /// <summary>
    /// Obtiene los límites (permitLimit, windowSeconds) para la política dada.
    /// </summary>
    private (int permitLimit, int windowSeconds) GetPolicyLimits(string policyName)
    {
        return policyName switch
        {
            PolicyNames.AuthEndpoints        => (_options.Policies.AuthEndpoints.PermitLimit,        _options.Policies.AuthEndpoints.WindowSeconds),
            PolicyNames.PredictionEndpoints  => (_options.Policies.PredictionEndpoints.PermitLimit,  _options.Policies.PredictionEndpoints.WindowSeconds),
            PolicyNames.AdminEndpoints       => (_options.Policies.AdminEndpoints.PermitLimit,       _options.Policies.AdminEndpoints.WindowSeconds),
            PolicyNames.PublicEndpoints      => (_options.Policies.PublicEndpoints.PermitLimit,      _options.Policies.PublicEndpoints.WindowSeconds),
            _                                => (_options.Policies.DefaultAuthenticated.PermitLimit, _options.Policies.DefaultAuthenticated.WindowSeconds),
        };
    }

    /// <summary>
    /// Construye la clave de partición para el contador:
    /// - Si la solicitud es de un usuario autenticado y ApplyByAuthenticatedUser está activo → usa el claim "sub".
    /// - Si es anónimo y ApplyByIpForAnonymous está activo → usa la IP remota.
    /// - En ambos casos se prefija con el nombre de la política para aislar contadores entre políticas.
    /// </summary>
    private string BuildPartitionKey(HttpContext context, string policyName)
    {
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User?.FindFirstValue("sub");

        if (_options.ApplyByAuthenticatedUser && !string.IsNullOrWhiteSpace(userId))
            return $"{policyName}:user:{userId}";

        if (_options.ApplyByIpForAnonymous)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return $"{policyName}:ip:{ip}";
        }

        return $"{policyName}:global";
    }

    /// <summary>
    /// Verifica si la solicitud está dentro del límite permitido para la clave dada.
    /// Utiliza el algoritmo de ventana fija en memoria con <see cref="ConcurrentDictionary{TKey,TValue}"/>.
    /// </summary>
    private static bool IsAllowed(string key, int permitLimit, int windowSeconds)
    {
        var now = DateTime.UtcNow;

        var entry = _counters.AddOrUpdate(
            key,
            // Crear nueva entrada
            _ => new RateLimitEntry { Count = 1, ExpiresAt = now.AddSeconds(windowSeconds) },
            // Actualizar entrada existente
            (_, existing) =>
            {
                if (now >= existing.ExpiresAt)
                {
                    // Ventana expirada: reiniciar contador
                    existing.Count     = 1;
                    existing.ExpiresAt = now.AddSeconds(windowSeconds);
                }
                else
                {
                    existing.Count++;
                }
                return existing;
            });

        return entry.Count <= permitLimit;
    }

    // ─── Clase interna de entrada del contador ────────────────────────────────

    /// <summary>
    /// Entrada del contador de rate limiting para una clave de partición.
    /// </summary>
    private sealed class RateLimitEntry
    {
        /// <summary>Número de solicitudes en la ventana activa.</summary>
        public int Count { get; set; }

        /// <summary>Fecha y hora de expiración de la ventana activa.</summary>
        public DateTime ExpiresAt { get; set; }
    }
}
