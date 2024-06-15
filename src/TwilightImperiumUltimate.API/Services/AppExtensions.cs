using Microsoft.EntityFrameworkCore;
using Serilog;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.Services;

public static class AppExtensions
{
    public static async Task CreateOrUpdateDbContextAsync(this WebApplication app)
    {
        var context = app.Services.GetRequiredService<TwilightImperiumDbContext>();
        await context.Database.MigrateAsync();
        await context.SeedDatabaseAsync();
    }

    public static async Task AppRun(this WebApplication app)
    {
        try
        {
            Log.Information("Starting Twilight Imperium Ultimate API");
            await app.RunAsync();
        }
        catch (InvalidOperationException ex)
        {
            Log.Fatal(ex, "Application Twilight Imperium Ultimate API start-up failed");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
