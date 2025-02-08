using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.DataAccess;
using TwilightImperiumUltimate.Draft;

namespace TwilightImperiumUltimate.Business;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDataAccessLayer(configuration);
        services.RegisterDraftServices();

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.RegisterAsyncPlayerServices();

        return services;
    }

    public static IServiceCollection RegisterAsyncPlayerServices(this IServiceCollection services)
    {
        services.AddScoped<IAsyncPlayerAllStatsFactory, AsyncPlayerAllStatsFactory>();
        services.AddScoped<IAsyncPlayerInfoFactory, AsyncPlayerInfoFactory>();
        services.AddScoped<IAsyncPlayerGameStatsFactory, AsyncPlayerGameStatsFactory>();
        services.AddScoped<IAsyncPlayerTurnStatsFactory, AsyncPlayerTurnStatsFactory>();
        services.AddScoped<IAsyncPlayerCombatStatsFactory, AsyncPlayerCombatStatsFactory>();
        services.AddScoped<IAsyncPlayerFactionStatsFactory, AsyncPlayerFactionStatsFactory>();
        services.AddScoped<IAsyncPlayerVpStatsFactory, AsyncPlayerVpStatsFactory>();
        services.AddScoped<IAsyncPlayerOpponentsInfoFactory, AsyncPlayerOpponentsInfoFactory>();
        services.AddScoped<IAsyncPlayerGamesFactory, AsyncPlayerGamesFactory>();

        services.AddScoped<IAsyncGeneralStatsFactory, AsyncGeneralStatsFactory>();
        services.AddScoped<IAsyncGamesStatsFactory, AsyncGamesStatsFactory>();
        services.AddScoped<IAsyncWinsStatsFactory, AsyncWinsStatsFactory>();
        services.AddScoped<IAsyncVpStatsFactory, AsyncVpStatsFactory>();
        services.AddScoped<IAsyncEliminationsStatsFactory, AsyncEliminationsStatsFactory>();
        services.AddScoped<IAsyncTurnsStatsFactory, AsyncTurnsStatsFactory>();
        services.AddScoped<IAsyncCombatStatsFactory, AsyncCombatStatsFactory>();
        services.AddScoped<IAsyncDurationsStatsFactory, AsyncDurationsStatsFactory>();
        services.AddScoped<IAsyncFactionStatsFactory, AsyncFactionStatsFactory>();
        services.AddScoped<IAsyncOpponentsStatsFactory, AsyncOpponentsStatsFactory>();
        services.AddScoped<IAsyncHistoryStatsFactory, AsyncHistoryStatsFactory>();

        return services;
    }
}
