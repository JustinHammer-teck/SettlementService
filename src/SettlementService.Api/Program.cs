using Microsoft.EntityFrameworkCore;
using SettlementService.Api.Configurations;
using SettlementService.Application;
using SettlementService.Infrastructure;
using SettlementService.Infrastructure.Persistent;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();
builder.AddApplication();
builder.AddInfrastructureConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await CleanDatabaseAsync(serviceProvider);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// this is a hack to clean the database on application startup to mimic InMemoryDatabase behavior
async Task CleanDatabaseAsync(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Clean Database on application on start up");

    // Disable change tracking for better performance
    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

    // Get all entity types from the DbContext
    var tables = context.Model.GetEntityTypes()
        .Select(e => e.GetTableName())
        .Distinct()
        .Where(t => !string.IsNullOrEmpty(t));

    foreach (var table in tables) await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");

    await context.SaveChangesAsync();
}

// A hack to use with CustomWebApplicationFactory in Application test.
public partial class Program
{
}