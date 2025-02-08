using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.PlayerInfo;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerAllStatsFactory(
    IAsyncPlayerInfoFactory asyncPlayerInfoFactory,
    IAsyncPlayerGameStatsFactory asyncPlayerGameStatsFactory,
    IAsyncPlayerTurnStatsFactory asyncPlayerTurnStatsFactory,
    IAsyncPlayerCombatStatsFactory asyncPlayerCombatStatsFactory,
    IAsyncPlayerFactionStatsFactory asyncPlayerFactionStatsFactory,
    IAsyncPlayerVpStatsFactory asyncPlayerVpStatsFactory,
    IAsyncPlayerOpponentsInfoFactory asyncPlayerOpponentsInfoFactory,
    IAsyncPlayerGamesFactory asyncPlayerGamesFactory,
    IMapper mapper)
    : IAsyncPlayerAllStatsFactory
{
    public async Task<AsyncPlayerProfileSummaryStatsDto> CreateAsyncPlayerStats(AsyncPlayerProfile playerProfile)
    {
        var playerInfo = await asyncPlayerInfoFactory.CreateAsyncPlayerInfoAsync(playerProfile);
        var profileSettings = mapper.Map<AsyncPlayerProfileSettingsDto>(playerProfile.ProfileSettings);
        var gameStats = await asyncPlayerGameStatsFactory.CreateAsyncPlayerGameStats(playerProfile);
        var turnStats = await asyncPlayerTurnStatsFactory.CreateAsyncPlayerTurnStats(playerProfile);
        var combatStats = await asyncPlayerCombatStatsFactory.CreateAsyncPlayerCombatStats(playerProfile);
        var factionStats = await asyncPlayerFactionStatsFactory.CreateAsyncPlayerFactionStats(playerProfile);
        var vpStats = await asyncPlayerVpStatsFactory.CreateAsyncPlayerVpStats(playerProfile);
        var opponentsStats = await asyncPlayerOpponentsInfoFactory.CreateAsyncPlayerOpponentsInfo(playerProfile);
        var games = await asyncPlayerGamesFactory.CreateAsyncPlayerGames(playerProfile);

        return new AsyncPlayerProfileSummaryStatsDto(
            playerInfo,
            profileSettings,
            gameStats,
            turnStats,
            combatStats,
            factionStats,
            vpStats,
            opponentsStats,
            games);
    }

    public Task<AsyncPlayerProfileSummaryStatsDto> CreateDefaultAsyncPlayerStats()
    {
        return Task.FromResult(new AsyncPlayerProfileSummaryStatsDto(
            new AsyncPlayerInfoDto(),
            new AsyncPlayerProfileSettingsDto(),
            new AsyncPlayerMainStatsSummaryDto(),
            new AsyncPlayerTurnStatsSummaryDto(),
            new AsyncPlayerCombatStatsSummaryDto(),
            new AsyncPlayerFactionStatsSummaryDto(),
            new AsyncPlayerVpStatsSummaryDto(),
            new AsyncPlayerOpponentsSummaryDto(),
            new AsyncPlayerGamesSummaryDto()));
    }
}
