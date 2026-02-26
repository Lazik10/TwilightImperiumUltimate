using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Services.Tigl;

public class TiglDataCache : ITiglDataCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly Dictionary<int, (IReadOnlyList<PlayerSeasonResultDto> Data, DateTimeOffset FetchedAt)> _leaderboards = [];

    private readonly Dictionary<(int Season, TiglLeague League), (IReadOnlyList<FactionSeasonStatsDto> Data, DateTimeOffset FetchedAt)> _factionStats = [];

    public IReadOnlyList<PlayerSeasonResultDto>? GetLeaderboard(int seasonNumber)
    {
        if (_leaderboards.TryGetValue(seasonNumber, out var entry) && DateTimeOffset.UtcNow - entry.FetchedAt <= CacheDuration)
            return entry.Data;

        return null;
    }

    public void UpdateLeaderboard(int seasonNumber, IReadOnlyList<PlayerSeasonResultDto> data)
    {
        _leaderboards[seasonNumber] = (data, DateTimeOffset.UtcNow);
    }

    public IReadOnlyList<FactionSeasonStatsDto>? GetFactionStats(int season, TiglLeague league)
    {
        var key = (season, league);
        if (_factionStats.TryGetValue(key, out var entry) && DateTimeOffset.UtcNow - entry.FetchedAt <= CacheDuration)
            return entry.Data;

        return null;
    }

    public void UpdateFactionStats(int season, TiglLeague league, IReadOnlyList<FactionSeasonStatsDto> data)
    {
        _factionStats[(season, league)] = (data, DateTimeOffset.UtcNow);
    }
}
