using Microsoft.EntityFrameworkCore;
using Serilog;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.API.Services;

internal static class AppExtensions
{
    public static async Task CreateOrUpdateDbContextAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var context = app.Services.GetRequiredService<TwilightImperiumDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task AppRunAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        try
        {
            Log.Information("Starting Twilight Imperium Ultimate API");

            // Register lifetime events to help diagnose unexpected shutdowns
            var lifetime = app.Services.GetService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
            if (lifetime is not null)
            {
                lifetime.ApplicationStarted.Register(() => Log.Information("Host application started"));
                lifetime.ApplicationStopping.Register(() => Log.Information("Host application stopping"));
                lifetime.ApplicationStopped.Register(() => Log.Information("Host application stopped"));
            }

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application Twilight Imperium Ultimate API start-up failed");
            throw;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
