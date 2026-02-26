using MediatR;
using Microsoft.Extensions.Caching.Memory;
using TwilightImperiumUltimate.Contracts.DTOs;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetTiglRankingsOverviewQueryHandler(
    IRankingsRepository rankingsRepository,
    IMemoryCache memoryCache)
    : IRequestHandler<GetTiglRankingsOverviewQuery, ItemListDto<RankingsUserDto>>
{
    private const string CacheKey = "rankings:overview";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly IRankingsRepository _repo = rankingsRepository;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<RankingsUserDto>> Handle(GetTiglRankingsOverviewQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<RankingsUserDto>? cached) && cached is not null)
            return cached;

        var rows = await _repo.GetUsersRankingsOverview(cancellationToken);

        var grouped = rows
            .GroupBy(r => new { r.TiglUserId, r.TiglUserName })
            .Select(g => new RankingsUserDto
            {
                TiglUserId = g.Key.TiglUserId,
                TiglUserName = g.Key.TiglUserName,
                Leagues = g.OrderBy(x => x.League).Select(x => new RankingsLeagueInfoDto
                {
                    League = x.League,
                    CurrentRank = x.HighestRank,
                    HighestPrestigeRank = x.HighestPrestigeRank,
                    HighestPrestigeRankLevel = x.HighestPrestigeRankLevel,
                    FactionPrestigeRankCount = x.FactionPrestigeRankCount,
                    LastAchievedAt = x.LastAchievedAt,
                    GamesPlayed = x.GamesPlayed,
                }).ToList(),
            })
            .OrderBy(u => u.TiglUserName)
            .ToList();

        var result = new ItemListDto<RankingsUserDto>(grouped);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
