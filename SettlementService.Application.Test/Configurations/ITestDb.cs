using System.Data.Common;

namespace SettlementService.Application.Test.Configurations;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    string GetConnectionString();

    Task ResetAsync();

    Task DisposeAsync();
}