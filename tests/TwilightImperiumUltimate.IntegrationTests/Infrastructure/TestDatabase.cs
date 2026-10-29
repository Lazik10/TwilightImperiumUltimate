using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.IntegrationTests.Infrastructure;

internal sealed class TestDatabase : IAsyncDisposable
{
    private readonly Func<ValueTask> cleanup;

    private TestDatabase(IDbContextFactory<TwilightImperiumDbContext> contextFactory, Func<ValueTask> cleanup)
    {
        this.ContextFactory = contextFactory;
        this.cleanup = cleanup;
    }

    public IDbContextFactory<TwilightImperiumDbContext> ContextFactory { get; }

    public static async Task<TestDatabase> CreateAsync(bool useMigrations = false, params IInterceptor[] interceptors)
    {
        const string serverName = @"(localdb)\MSSQLLocalDB";

        var databaseName = $"DraftRoomsTests_{Guid.NewGuid():N}";
        await CreateSqlServerDatabaseAsync(serverName, databaseName);

        var connectionString = CreateSqlServerConnectionString(serverName, databaseName);
        var options = new DbContextOptionsBuilder<TwilightImperiumDbContext>()
            .UseSqlServer(connectionString)
            .AddInterceptors(interceptors)
            .Options;
        var database = new TestDatabase(
            new TestDbContextFactory(options),
            () => DropSqlServerDatabaseAsync(serverName, databaseName));

        try
        {
            await using var context = await database.ContextFactory.CreateDbContextAsync();
            if (useMigrations)
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            return database;
        }
        catch
        {
            await database.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await this.cleanup();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security",
        "CA2100:Review SQL queries for security vulnerabilities",
        Justification = "The database name is internally generated from a GUID and cannot contain SQL syntax.")]
    private static async Task CreateSqlServerDatabaseAsync(string serverName, string databaseName)
    {
        await using var connection = new SqlConnection(CreateSqlServerConnectionString(serverName, "master"));
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE [{databaseName}]";
        await command.ExecuteNonQueryAsync();

        // Pooling is disabled for teardown; LocalDB must not close the database between fixture operations.
        command.CommandText = $"ALTER DATABASE [{databaseName}] SET AUTO_CLOSE OFF";
        await command.ExecuteNonQueryAsync();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security",
        "CA2100:Review SQL queries for security vulnerabilities",
        Justification = "The database name is internally generated from a GUID and cannot contain SQL syntax.")]
    private static async ValueTask DropSqlServerDatabaseAsync(string serverName, string databaseName)
    {
        await using var connection = new SqlConnection(CreateSqlServerConnectionString(serverName, "master"));
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{databaseName}]";
        await command.ExecuteNonQueryAsync();
    }

    private static string CreateSqlServerConnectionString(string serverName, string databaseName)
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = serverName,
            InitialCatalog = databaseName,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            Pooling = false,
        }.ConnectionString;
    }

    private sealed class TestDbContextFactory(DbContextOptions<TwilightImperiumDbContext> options)
        : IDbContextFactory<TwilightImperiumDbContext>
    {
        public TwilightImperiumDbContext CreateDbContext()
        {
            return new TwilightImperiumDbContext(options);
        }
    }
}
