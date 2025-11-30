using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetSeasonLeaderboardQueryHandler(
    IDbContextFactory<TwilightImperiumDbContext> contextFactory,
    IMemoryCache cache)
    : IRequestHandler<GetSeasonLeaderboardQuery, ItemListDto<PlayerSeasonResultDto>>
{
    private const string CacheKeyPrefix = "season-leaderboard-";

    public async Task<ItemListDto<PlayerSeasonResultDto>> Handle(GetSeasonLeaderboardQuery request, CancellationToken cancellationToken)
    {
        using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

        // Determine current active season number
        var activeSeasonNumber = await db.Seasons
            .Where(s => s.IsActive)
            .Select(s => s.SeasonNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var isActiveSeasonRequest = request.Season == activeSeasonNumber && activeSeasonNumber != 0;

        if (isActiveSeasonRequest)
        {
            var cacheKey = CacheKeyPrefix + request.Season;
            if (cache.TryGetValue(cacheKey, out ItemListDto<PlayerSeasonResultDto>? cachedResult))
            {
                return cachedResult;
            }
        }

        var data = await db.SeasonLeaderboard
            .Where(x => x.Season == request.Season)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtos = data.Select(e => new PlayerSeasonResultDto
        {
            TiglUserId = e.TiglUserId,
            TiglUserName = e.TiglUserName,
            TiglDiscordTag = e.TiglDiscordTag,
            Season = e.Season,
            League = e.League,
            GamesPlayed = e.GamesPlayed,
            WinPercentage = e.WinPercentage,
            AsyncRating = e.AsyncRating,
            AussieScore = e.AussieScore,
            GlickoRating = e.GlickoRating,
            GlickoRd = e.GlickoRd,
            GlickoVolatility = e.GlickoVolatility,
            TrueSkillMu = e.TrueSkillMu,
            TrueSkillSigma = e.TrueSkillSigma,
            TrueSkillConservativeRating = e.TrueSkillConservativeRating,
        }).ToList();

        var result = new ItemListDto<PlayerSeasonResultDto>(dtos);

        if (isActiveSeasonRequest)
        {
            cache.Set(CacheKeyPrefix + request.Season, result, TimeSpan.FromHours(1));
        }

        return result;
    }
}
