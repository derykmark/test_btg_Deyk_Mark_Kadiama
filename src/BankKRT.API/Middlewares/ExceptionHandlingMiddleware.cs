using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BankKRT.API.Middlewares;

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
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Error";
                problemDetails.Detail = "One or more validation errors occurred.";
                problemDetails.Extensions["errors"] = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                break;
            case KeyNotFoundException keyNotFoundException:
                response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Detail = keyNotFoundException.Message;
                break;
            case InvalidOperationException invalidOperationException:
                response.StatusCode = StatusCodes.Status409Conflict;
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Conflict";
                problemDetails.Detail = invalidOperationException.Message;
                break;
            default:
                response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = "An unexpected error occurred.";
                break;
        }

        var result = JsonSerializer.Serialize(problemDetails);
        await response.WriteAsync(result);
    }
}
