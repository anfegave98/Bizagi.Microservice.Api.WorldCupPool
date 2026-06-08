using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Bizagi.Microservice.Api.WorldCupPool.Middlewares;

/// <summary>
/// Middleware global de manejo de excepciones.
/// Convierte las excepciones en respuestas HTTP controladas sin exponer
/// información interna como stack traces o mensajes sensibles.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware de excepciones.
    /// </summary>
    /// <param name="next">Siguiente delegado en el pipeline.</param>
    /// <param name="logger">Logger del middleware.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Intercepta la solicitud HTTP y captura cualquier excepción no controlada.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
            InvalidOperationException e when e.Message.Contains("inactivo") => (HttpStatusCode.Forbidden, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno. Por favor intente más tarde.")
        };

        // Solo registrar stack trace en errores internos; los errores de negocio no son críticos
        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Error interno no controlado en la solicitud {Method} {Path}.",
                context.Request.Method, context.Request.Path);
        else
            _logger.LogWarning("Error controlado [{StatusCode}]: {Message}", (int)statusCode, message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
