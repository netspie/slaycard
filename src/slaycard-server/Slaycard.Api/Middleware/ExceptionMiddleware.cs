using FluentValidation;
using Slaycard.Api.Core.Auth;
using Slaycard.Api.Core.Domain;
using System.Net;

namespace Slaycard.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            await HandleExceptionAsync(_logger, context, ex);
        }
    }

    private static Task HandleExceptionAsync(
        ILogger logger, HttpContext context, Exception exception)
    {
        var statusCode = (int) (
        exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            DomainException => HttpStatusCode.BadRequest,
            FileNotFoundException => HttpStatusCode.NotFound,
            NotAuthorizedException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError,
        });

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var error = new ApiError(
            statusCode, exception.Message);

        return context.Response.WriteAsJsonAsync(error);
    }
}

public record ApiError(int Status, string Message);