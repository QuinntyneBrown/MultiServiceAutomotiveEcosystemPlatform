using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTOs.Common;

namespace Shared.Infrastructure.Middleware;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail("Validation failed", validationException.Errors.Select(e => e.ErrorMessage))
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                ApiResponse.Fail("Resource not found")
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                ApiResponse.Fail("Unauthorized")
            ),
            ArgumentException argEx => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail(argEx.Message)
            ),
            InvalidOperationException opEx => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail(opEx.Message)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse.Fail("An unexpected error occurred")
            )
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
        }
        else
        {
            _logger.LogWarning(exception, "A handled exception occurred: {Message}", exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
