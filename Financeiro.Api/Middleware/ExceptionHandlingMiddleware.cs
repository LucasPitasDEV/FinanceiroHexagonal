using System.Net;
using System.Text.Json;

namespace Financeiro.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public class ErrorResponse
    {
        public string? Mensagem { get; set; }
        public string? Detalhes { get; set; }
    }

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            // Erros de Validação de Negócio (Domínio) -> 400
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Erro de Validação");
        }
        catch (Exception ex)
        {
            // Erros Inesperados -> 500
            _logger.LogError(ex, "Ocorreu um erro inesperado.");
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Erro Interno no Servidor");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code, string title)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var result = JsonSerializer.Serialize(new ErrorResponse
        {
            Mensagem = exception.Message,
            Detalhes = title
        });

        return context.Response.WriteAsync(result);
    }
}
