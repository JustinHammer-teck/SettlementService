using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SettlementService.Application.Common.Interfaces;
using SettlementService.Infrastructure.Persistent;

namespace SettlementService.Infrastructure;

public static class InfrastructureConfiguration
{
    public static void AddInfrastructureConfiguration(this IHostApplicationBuilder builder)
    {
         
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionString")));

        builder.Services.AddScoped<IApplicationDbContenxt, ApplicationDbContext>();
    }
}