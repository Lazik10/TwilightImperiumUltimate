using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;
using TwilightImperiumUltimate.Core.Entities.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public class AsyncPlayerOpponentsInfoFactory(
    IAsyncStatsRepository asyncStatsRepository)
    : IAsyncPlayerOpponentsInfoFactory
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly string _privateProfile = "Private Profile";

    public async Task<AsyncPlayerOpponentsSummaryDto> CreateAsyncPlayerOpponentsInfo(AsyncPlayerProfile playerProfile)
    {
        var games = playerProfile.GameStatistics.Select(x => x.GameStats).ToList();

        var tiglGames = games.Where(x => x.IsTigl).ToList();
        var customGames = games.Where(x => !x.IsTigl).ToList();

        var playerProfiles = await _asyncStatsRepository.GetAllAsyncPlayerProfiles(false, CancellationToken.None);

        var allOpponentsDto = GetOpponents(games, playerProfile.DiscordUserId, playerProfiles);
        var tiglOpponentsDto = GetOpponents(tiglGames, playerProfile.DiscordUserId, playerProfiles);
        var customOpponentsDto = GetOpponents(customGames, playerProfile.DiscordUserId, playerProfiles);

        var summary = new AsyncPlayerOpponentsSummaryDto(allOpponentsDto, tiglOpponentsDto, customOpponentsDto);

        return summary;
    }

    private List<AsyncPlayerOpponentInfoDto> GetOpponents(List<GameStats> games, long discordUserId, List<AsyncPlayerProfile> playerProfiles)
    {
        var opponents = games
            .Where(x => x.PlayerStatistics.Any(y => y.DiscordUserID == discordUserId))
            .SelectMany(x => x.PlayerStatistics)
            .ToList();

        return opponents
            .Where(player => player.DiscordUserID != discordUserId)
            .GroupBy(player => player.DiscordUserID)
            .Select(group =>
                new AsyncPlayerOpponentInfoDto(
                    IsExcludedProfile(group.Key, playerProfiles) ? 0 : playerProfiles.Find(x => x.DiscordUserId == group.Key)?.Id ?? 0,
                    IsExcludedProfile(group.Key, playerProfiles) ? _privateProfile : group.First().DiscordUserName,
                    group.Count()))
            .OrderByDescending(x => x.Games)
            .Take(20)
            .ToList();
    }

    private bool IsExcludedProfile(long playerDiscordId, List<AsyncPlayerProfile> playerProfiles)
    {
        var profile = playerProfiles.Find(y => y.DiscordUserId == playerDiscordId);
        if (profile is null)
            return true;

        return !profile.ProfileSettings?.ShowOpponents ?? true;
    }
}
