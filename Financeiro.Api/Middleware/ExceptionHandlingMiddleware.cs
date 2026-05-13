using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

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
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Violação de Regra de Negócio");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro não tratado.");
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Erro Interno no Servidor");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code, string title)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)code;

        var problemDetails = new ProblemDetails
        {
            Status = (int)code,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        var result = System.Text.Json.JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(result);
    }
}
