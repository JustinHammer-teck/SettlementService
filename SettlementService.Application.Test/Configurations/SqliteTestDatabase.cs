using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SettlementService.Infrastructure.Persistent;

namespace SettlementService.Application.Test.Configurations;

public class SqliteTestDatabase : ITestDatabase
{
    private readonly SqliteConnection _connection;
    private readonly string _connectionString;

    public SqliteTestDatabase()
    {
        _connectionString = "DataSource=:memory:";
        _connection = new SqliteConnection(_connectionString);
    }

    public async Task InitialiseAsync()
    {
        if (_connection.State == ConnectionState.Open) await _connection.CloseAsync();

        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.Migrate();
        await context.Database.EnsureCreatedAsync();
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public async Task ResetAsync()
    {
        await InitialiseAsync();
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}