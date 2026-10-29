using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TwilightImperiumUltimate.API.Services.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.Hosting;

internal sealed class TestApiFactory(TestDatabase database, TimeProvider timeProvider, string? frontendOrigin = null) : WebApplicationFactory<Program>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "CA2213:Disposable fields should be disposed",
        Justification = "The caller owns the shared database and disposes it after all API hosts have stopped.")]
    private readonly TestDatabase _database = database
        ?? throw new ArgumentNullException(nameof(database));

    private readonly TimeProvider _timeProvider = timeProvider
        ?? throw new ArgumentNullException(nameof(timeProvider));

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        if (frontendOrigin is not null)
        {
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { ["Frontend:LocalUrl"] = frontendOrigin }));
        }

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<TwilightImperiumDbContext>();
            services.RemoveAll<DbContextOptions<TwilightImperiumDbContext>>();
            services.RemoveAll<IDbContextFactory<TwilightImperiumDbContext>>();
            services.AddSingleton<IDbContextFactory<TwilightImperiumDbContext>>(this._database.ContextFactory);
            services.AddScoped<TwilightImperiumDbContext>(serviceProvider =>
                serviceProvider
                    .GetRequiredService<IDbContextFactory<TwilightImperiumDbContext>>()
                    .CreateDbContext());

            services.RemoveAll<TimeProvider>();
            services.AddSingleton(this._timeProvider);

            services.AddSingleton<IStartupFilter, TestRemoteIpStartupFilter>();

            services.RemoveAll<IHostedService>();
            services.AddHostedService<PresenceChatPublisher>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthenticationHandler.SchemeName;
                    options.DefaultScheme = TestAuthenticationHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.SchemeName,
                    _ => { });
        });
    }
}
