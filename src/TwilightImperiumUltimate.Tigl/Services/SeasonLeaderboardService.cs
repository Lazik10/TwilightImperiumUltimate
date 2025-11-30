using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Tigl.Services;

public class SeasonLeaderboardService(IDbContextFactory<TwilightImperiumDbContext> contextFactory) : ISeasonLeaderboardService
{
    public async Task CreateLeaderboard(int seasonEnded, CancellationToken cancellationToken)
    {
        using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

        if (await db.SeasonLeaderboard.AnyAsync(x => x.Season == seasonEnded, cancellationToken))
            return;

        // Load only basic Tigl user info
        var allTiglUsers = await db.TiglUsers
            .Select(u => new { u.Id, u.TiglUserName, u.DiscordTag, u.DiscordId })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Preload ratings per league with minimal projections
        var asyncRatings = await db.AsyncStats
            .Select(s => new { s.TiglUserId, s.League, s.Rating!.Rating, s.Rating!.AussieScore })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var glickoRatings = await db.GlickoStats
            .Select(s => new { s.TiglUserId, s.League, s.Rating!.Rating, s.Rating!.Rd, s.Rating!.Volatility })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var trueSkillRatings = await db.TrueSkillStats
            .Select(s => new { s.TiglUserId, s.League, s.TrueSkillRating!.Rating, s.TrueSkillRating!.Sigma, s.TrueSkillRating!.ConservativeRating })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Compute GamesPlayed and Wins on server side and materialize compact dictionary
        var gameWinAggregates = await db.GameReports
            .AsNoTracking()
            .SelectMany(gr => gr.PlayerResults.Select(pr => new { gr.League, pr.TiglUserId, pr.IsWinner }))
            .GroupBy(x => new { x.League, x.TiglUserId })
            .Select(g => new
            {
                g.Key.League,
                g.Key.TiglUserId,
                GamesPlayed = g.Count(),
                Wins = g.Count(r => r.IsWinner),
            })
            .ToListAsync(cancellationToken);

        // Build fast lookup maps
        var asyncByLeagueUser = asyncRatings
            .GroupBy(x => x.League)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(x => x.TiglUserId, x => new { x.Rating, x.AussieScore }));

        var glickoByLeagueUser = glickoRatings
            .GroupBy(x => x.League)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(x => x.TiglUserId, x => new { x.Rating, x.Rd, x.Volatility }));

        var trueSkillByLeagueUser = trueSkillRatings
            .GroupBy(x => x.League)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(x => x.TiglUserId, x => new { x.Rating, x.Sigma, x.ConservativeRating })
            );

        var aggregatesByLeague = gameWinAggregates
            .GroupBy(x => x.League)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(x => x.TiglUserId, x => new { x.GamesPlayed, x.Wins })
            );

        foreach (var league in Enum.GetValues<TiglLeague>().Except([TiglLeague.Test]))
        {
            var rows = new List<PlayerSeasonResult>();

            foreach (var u in allTiglUsers)
            {
                var asyncRating = asyncByLeagueUser.TryGetValue(league, out var aMap) && aMap.TryGetValue(u.Id, out var aR)
                    ? aR
                    : null;
                var glickoRating = glickoByLeagueUser.TryGetValue(league, out var gMap) && gMap.TryGetValue(u.Id, out var gR)
                    ? gR
                    : null;
                var trueskillRating = trueSkillByLeagueUser.TryGetValue(league, out var tMap) && tMap.TryGetValue(u.Id, out var tR)
                    ? tR
                    : null;

                int gamesPlayed = 0;
                int wins = 0;

                if (aggregatesByLeague.TryGetValue(league, out var agMap) && agMap.TryGetValue(u.Id, out var aggr))
                {
                    gamesPlayed = aggr.GamesPlayed;
                    wins = aggr.Wins;
                }

                var row = new PlayerSeasonResult
                {
                    Season = seasonEnded,
                    TiglUserId = u.Id,
                    TiglUserName = u.TiglUserName,
                    TiglDiscordTag = u.DiscordTag,
                    League = league,
                    GamesPlayed = gamesPlayed,
                    WinPercentage = gamesPlayed == 0 ? 0 : Math.Round((wins / (double)gamesPlayed) * 100.0, 4),

                    AsyncRating = asyncRating?.Rating ?? 1000.0,
                    AussieScore = asyncRating?.AussieScore ?? 0.0,

                    GlickoRating = glickoRating?.Rating ?? 1500.0,
                    GlickoRd = glickoRating?.Rd ?? 350.0,
                    GlickoVolatility = glickoRating?.Volatility ?? 0.06,

                    TrueSkillMu = trueskillRating?.Rating ?? 25.0,
                    TrueSkillSigma = trueskillRating?.Sigma ?? 8.333,
                    TrueSkillConservativeRating = trueskillRating?.ConservativeRating ?? 0.0,

                    IsActive = gamesPlayed > 0,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                };

                if (gamesPlayed > 0)
                    rows.Add(row);
            }

            if (rows.Count != 0)
            {
                db.SeasonLeaderboard.AddRange(rows);
                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
