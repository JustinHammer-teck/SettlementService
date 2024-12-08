using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SettlementService.Application.Exceptions;

namespace SettlementService.Api.Common;

public class GlobalCustomExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalCustomExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        return exception switch
        {
            ValidationException validationException => await ValidationExceptionHandler(httpContext, exception,
                validationException),
            RequestConflictException requestConflictException => await RequestConflictExceptionHandler(httpContext,
                exception, requestConflictException),
            NotFoundException notFoundException => await NotFoundExceptionHandler(httpContext, exception,
                notFoundException),
            _ => false
        };
    }

    private async Task<bool> NotFoundExceptionHandler(
        HttpContext httpContext,
        Exception exception,
        NotFoundException notFoundException)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = notFoundException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
    }

    private async Task<bool> ValidationExceptionHandler(
        HttpContext httpContext,
        Exception exception,
        ValidationException validationException)
    {
        var problemDetails = new ValidationProblemDetails(validationException.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Exception",
            Detail = validationException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
    }

    private async Task<bool> RequestConflictExceptionHandler(
        HttpContext httpContext,
        Exception exception,
        RequestConflictException requestConflictException)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Status = StatusCodes.Status409Conflict,
            Title = requestConflictException.Type,
            Detail = exception.Message,
            Extensions =
            {
                { "code", requestConflictException.Command }
            }
        };

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
    }
}