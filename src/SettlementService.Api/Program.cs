using SettlementService.Api.Configurations;
using SettlementService.Application;
using SettlementService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();
builder.AddApplication();
builder.AddInfrastructureConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();