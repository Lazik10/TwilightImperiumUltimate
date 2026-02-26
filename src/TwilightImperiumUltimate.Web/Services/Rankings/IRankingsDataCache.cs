using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Web.Services.Rankings;

public interface IRankingsDataCache
{
    IReadOnlyCollection<RankingsUserDto>? Rankings { get; }

    IReadOnlyCollection<RankingsLeaderDto>? Leaders { get; }

    bool IsRankingsExpired { get; }

    bool IsLeadersExpired { get; }

    void Update(IReadOnlyCollection<RankingsUserDto> rankings);

    void Update(IReadOnlyCollection<RankingsLeaderDto> leaders);
}
