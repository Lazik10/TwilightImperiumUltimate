using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Web.Services.Rankings;

public class RankingsDataCache : IRankingsDataCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);
    private DateTimeOffset _rankingsLastFetched = DateTimeOffset.MinValue;
    private DateTimeOffset _leadersLastFetched = DateTimeOffset.MinValue;

    public IReadOnlyCollection<RankingsUserDto>? Rankings { get; private set; }

    public IReadOnlyCollection<RankingsLeaderDto>? Leaders { get; private set; }

    public bool IsRankingsExpired => Rankings is null || DateTimeOffset.UtcNow - _rankingsLastFetched > CacheDuration;

    public bool IsLeadersExpired => Leaders is null || DateTimeOffset.UtcNow - _leadersLastFetched > CacheDuration;

    public void Update(IReadOnlyCollection<RankingsUserDto> rankings)
    {
        Rankings = rankings;
        _rankingsLastFetched = DateTimeOffset.UtcNow;
    }

    public void Update(IReadOnlyCollection<RankingsLeaderDto> leaders)
    {
        Leaders = leaders;
        _leadersLastFetched = DateTimeOffset.UtcNow;
    }
}
