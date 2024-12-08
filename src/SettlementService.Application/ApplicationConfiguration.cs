using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SettlementService.Application.Common.BehaviorPipeline;
using SettlementService.Domain.Entities;

namespace SettlementService.Application;

public static class ApplicationConfiguration
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<OfficeHour>();
        builder.Services.AddSingleton<BookingAvailableHour>();

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
    }
}