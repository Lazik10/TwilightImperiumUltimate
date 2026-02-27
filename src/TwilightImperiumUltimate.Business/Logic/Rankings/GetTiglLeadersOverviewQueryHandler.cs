using Microsoft.Extensions.Caching.Memory;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetTiglLeadersOverviewQueryHandler(
    IRankingsRepository rankingsRepository,
    IMemoryCache memoryCache)
    : IRequestHandler<GetTiglLeadersOverviewQuery, ItemListDto<RankingsLeaderDto>>
{
    private const string CacheKey = "rankings:leaders";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private readonly IRankingsRepository _repo = rankingsRepository;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<ItemListDto<RankingsLeaderDto>> Handle(GetTiglLeadersOverviewQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_memoryCache.TryGetValue(CacheKey, out ItemListDto<RankingsLeaderDto>? cached) && cached is not null)
            return cached;

        var rows = await _repo.GetLeadersOverview(cancellationToken);
        var result = new ItemListDto<RankingsLeaderDto>(rows);
        _memoryCache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}
