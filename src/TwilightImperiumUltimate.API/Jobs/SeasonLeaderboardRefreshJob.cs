using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quartz;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.Tigl.Services;

namespace TwilightImperiumUltimate.API.Jobs;

public class SeasonLeaderboardRefreshJob(
    ILogger<SeasonLeaderboardRefreshJob> logger,
    IDbContextFactory<TwilightImperiumDbContext> contextFactory,
    ISeasonLeaderboardService seasonLeaderboardService,
    IMemoryCache cache)
    : IJob
{
    private readonly ILogger<SeasonLeaderboardRefreshJob> _logger = logger;
    private readonly IDbContextFactory<TwilightImperiumDbContext> _contextFactory = contextFactory;
    private readonly ISeasonLeaderboardService _seasonLeaderboardService = seasonLeaderboardService;
    private readonly IMemoryCache _cache = cache;

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var db = await _contextFactory.CreateDbContextAsync(context.CancellationToken);
            var activeSeason = await db.Seasons
                .Where(s => s.IsActive)
                .Select(s => s.SeasonNumber)
                .FirstOrDefaultAsync(context.CancellationToken);

            if (activeSeason == 0)
            {
                _logger.LogWarning("Season leaderboard refresh skipped: no active season found.");
                return;
            }

            _logger.LogInformation("Season leaderboard refresh started for season {Season}", activeSeason);

            // Remove existing snapshot rows for current active season
            var existing = db.SeasonLeaderboard.Where(r => r.Season == activeSeason);
            db.SeasonLeaderboard.RemoveRange(existing);
            await db.SaveChangesAsync(context.CancellationToken);

            // Recreate snapshot (service will insert rows)
            await _seasonLeaderboardService.CreateLeaderboard(activeSeason, context.CancellationToken);

            // Invalidate cache so next request rebuilds it
            _cache.Remove("season-leaderboard-" + activeSeason);

            _logger.LogInformation("Season leaderboard refresh completed for season {Season}", activeSeason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing season leaderboard.");
        }
    }
}
