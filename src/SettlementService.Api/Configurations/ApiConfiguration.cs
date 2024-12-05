using Microsoft.AspNetCore.Http.Features;
using SettlementService.Api.Common;

namespace SettlementService.Api.Configurations;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                
                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        builder.Services.AddExceptionHandler<GlobalCustomExceptionHandler>();

        builder.Logging.AddOpenTelemetry();
    }
}