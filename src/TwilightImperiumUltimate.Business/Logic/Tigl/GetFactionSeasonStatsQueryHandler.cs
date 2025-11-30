using Microsoft.Extensions.Caching.Memory;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetFactionSeasonStatsQueryHandler(
    IDbContextFactory<TwilightImperiumDbContext> contextFactory,
    IMemoryCache cache)
    : IRequestHandler<GetFactionSeasonStatsQuery, ItemListDto<FactionSeasonStatsDto>>
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _contextFactory = contextFactory;
    private readonly IMemoryCache _cache = cache;

    public async Task<ItemListDto<FactionSeasonStatsDto>> Handle(GetFactionSeasonStatsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"factionStats:{request.Season}:{request.League}";
        if (_cache.TryGetValue(cacheKey, out ItemListDto<FactionSeasonStatsDto>? cached))
            return cached!;

        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var query = db.GameReports
            .AsNoTracking()
            .Where(gr => (request.Season == -1 || gr.Season == request.Season) && gr.League == request.League)
            .SelectMany(gr => gr.PlayerResults.Select(pr => new { gr.Id, gr.Season, gr.League, pr.Faction, pr.IsWinner, pr.Score }))
            .GroupBy(x => x.Faction)
            .Select(g => new FactionSeasonStatsDto
            {
                Faction = g.Key,
                GamesPlayed = g.Count(),
                Wins = g.Count(x => x.IsWinner),
                WinRate = g.Count(x => x.IsWinner) == 0 ? 0 : (double)g.Count(x => x.IsWinner) / g.Count() * 100,
                TotalScore = g.Sum(x => x.Score),
                AverageScore = g.Average(x => x.Score),
            })
            .OrderByDescending(x => x.Wins)
            .ThenByDescending(x => x.GamesPlayed);

        var list = await query.ToListAsync(cancellationToken);
        var result = new ItemListDto<FactionSeasonStatsDto>(list);

        var options = new MemoryCacheEntryOptions();
        if (request.Season == -1)
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        _cache.Set(cacheKey, result, options);
        return result;
    }
}
