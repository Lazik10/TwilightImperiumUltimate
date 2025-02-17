using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncCombatStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncCombatStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncCombatSummaryStatsDto> CreateAsyncCombatStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var allGames = games
            .Where(x =>
                x.EndedTimestamp != null
                && x.HasWinner
                && x.Scoreboard >= 10
                && x.Scoreboard <= 14
                && x.NumberOfPlayers >= 6
                && x.NumberOfPlayers <= 8
                && !x.AbsolMode
                && !x.FrankenGame)
            .ToList();
        var tiglGames = allGames.Where(x => x.IsTigl).ToList();
        var customGames = allGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateCombatStats(allGames, playerProfiles, limit);
        var tiglGameStats = CreateCombatStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateCombatStats(customGames, playerProfiles, limit);

        return new AsyncCombatSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncCombatStatsDto CreateCombatStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playerDtos = games
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncCombatPlayerDto(
                playerInfo.Id,
                playerInfo.Name,
                g.Count(),
                g.Sum(x => x.ActualHits),
                g.Sum(x => x.ExpectedHits),
                g.Max(x => x.ActualHits));
            })
            .Where(x => x.Games >= 20)
            .ToList();

        var totalHitsPlayers = playerDtos
            .OrderByDescending(x => x.Hits)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var maxHitsPerGamePlayers = playerDtos
            .OrderByDescending(x => x.MaxHitPerGame)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var maxAverageHitsPerGamePlayers = playerDtos
            .OrderByDescending(x => x.AverageHits)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var bestHitsDeviationPlayers = playerDtos
            .Where(x => x.HitsDeviation > 0)
            .OrderByDescending(x => x.HitsDeviation)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var worstHitsDeviationPlayers = playerDtos
            .Where(x => x.HitsDeviation < 0)
            .OrderBy(x => x.HitsDeviation)
            .ThenBy(x => x.Games)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncCombatStatsDto(
            totalHitsPlayers,
            maxHitsPerGamePlayers,
            maxAverageHitsPerGamePlayers,
            bestHitsDeviationPlayers,
            worstHitsDeviationPlayers);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowCombatStats && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
