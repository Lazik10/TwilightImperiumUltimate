using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncWinsStatsFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncWinsStatsFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<AsyncWinsSummaryStatsDto> CreateAsyncWinsStatsSummary(int limit, CancellationToken cancellationToken)
    {
        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(true, cancellationToken);

        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);

        var allFinishedGames = games.Where(x => x.EndedTimestamp != null && x.HasWinner).ToList();
        var tiglGames = allFinishedGames.Where(x => x.IsTigl).ToList();
        var customGames = allFinishedGames.Where(x => !x.IsTigl).ToList();

        var allGameStats = CreateWinsStats(allFinishedGames, playerProfiles, limit);
        var tiglGameStats = CreateWinsStats(tiglGames, playerProfiles, limit);
        var customGameStats = CreateWinsStats(customGames, playerProfiles, limit);

        return new AsyncWinsSummaryStatsDto(allGameStats, tiglGameStats, customGameStats);
    }

    private AsyncWinsStatsDto CreateWinsStats(List<GameStats> games, List<AsyncPlayerProfile> playerProfiles, int limit)
    {
        var playersDto = games
            .SelectMany(x => x.PlayerStatistics)
            .GroupBy(x => x.DiscordUserID)
            .Where(x => x.Count() >= 20)
            .Select(g =>
            {
                var playerInfo = GetPlayerInfo(g.Key, playerProfiles);

                return new AsyncWinsPlayerDto(
                playerInfo.Id,
                playerInfo.Name,
                g.Count(y => y.Winner),
                g.Count());
            })
            .ToList();

        var playersWithMostWins = playersDto
            .Where(x => x.Wins > 0)
            .OrderByDescending(g => g.Wins)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var playersWithHighestWinsPercentage = playersDto
            .Where(x => x.WinPercentage > 0)
            .OrderByDescending(g => g.WinPercentage)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        var playersWithHighestWinsDeviation = playersDto
            .Where(x => x.WinDeviation > 0)
            .OrderByDescending(g => g.WinDeviation)
            .ThenBy(x => x.UserName)
            .Take(limit)
            .ToList();

        return new AsyncWinsStatsDto(playersWithMostWins, playersWithHighestWinsPercentage, playersWithHighestWinsDeviation);
    }

    private (int Id, string Name) GetPlayerInfo(long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var player = playerProfiles.Find(x => x.DiscordUserId == discordUserId);

        if (player is not null && player.ProfileSettings is not null && player.ProfileSettings.ShowWinRate && !player.ProfileSettings.ExcludeFromAsyncStats)
            return (player.Id, player.DiscordUserName);

        return (0, StringConstants.PrivateProfile);
    }
}
