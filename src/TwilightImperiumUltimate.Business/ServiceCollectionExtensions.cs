using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwilightImperiumUltimate.Business.Services.Async.Implementations;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft;
using TwilightImperiumUltimate.Tigl;

namespace TwilightImperiumUltimate.Business;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.RegisterDataAccessLayer(configuration);
        services.RegisterDraftServices();
        services.RegisterDraftRoomServices();
        services.RegisterTiglServices();
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
        services.AddScoped<IAsyncStatisticsSnapshotBuilder, AsyncStatisticsSnapshotBuilder>();
        services.AddScoped<IAsyncStatisticsSnapshotReader, AsyncStatisticsSnapshotReader>();

        return services;
    }

    public static IServiceCollection RegisterDraftRoomServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IRoomCodeGenerator, RoomCodeGenerator>();
        services.AddSingleton<IGuestSecretService, GuestSecretService>();
        services.AddSingleton<IRoomPasswordHasher, RoomPasswordHasher>();
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        services.AddSingleton<IJoinAttemptThrottle, JoinAttemptThrottle>();
        services.AddSingleton<IDraftRoomChatThrottle, DraftRoomChatThrottle>();
        services.AddSingleton<IPresenceTracker, PresenceTracker>();
        services.TryAddSingleton<IRoomChangeNotifier, NullRoomChangeNotifier>();
        services.AddScoped<IDraftRoomDisplayNameValidator, DraftRoomDisplayNameValidator>();
        services.AddScoped<IDraftRoomAuthorizer, DraftRoomAuthorizer>();
        services.AddSingleton<IDraftRoomLog, DraftRoomLog>();
        services.AddSingleton<IDraftSeatOccupantChangeHandler, FactionPickOccupantChangeHandler>();
        services.AddScoped<IFactionPickPoolProvider, FactionPickPoolProvider>();
        services.AddScoped<IDraftRoomDraftPersistence, DraftRoomDraftPersistence>();
        services.AddScoped<DraftRoomResultWriter>();
        services.AddScoped<DraftRoomCleanupService>();
        services.AddScoped<IDraftRoomDraftViewProvider, DraftRoomDraftViewProvider>();

        return services;
    }
}
