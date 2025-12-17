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
            .Select(s => new
            {
                s.TiglUserId,
                s.League,
                s.Rating!.Rating,
                s.Rating!.AussieScore,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var glickoRatings = await db.GlickoStats
            .Select(s => new
            {
                s.TiglUserId,
                s.League,
                s.Rating!.Rating,
                s.Rating!.Rd,
                s.Rating!.Volatility,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var trueSkillRatings = await db.TrueSkillStats
            .Select(s => new
            {
                s.TiglUserId,
                s.League,
                s.TrueSkillRating!.Rating,
                s.TrueSkillRating!.Sigma,
                s.TrueSkillRating!.ConservativeRating,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Compute per-league aggregates
        var gameWinAggregates = await db.GameReports
            .AsNoTracking()
            .SelectMany(gr => gr.PlayerResults.Select(pr => new
            {
                gr.League,
                gr.Season,
                pr.TiglUserId,
                pr.IsWinner,
            }))
            .GroupBy(x => new { x.League, x.TiglUserId })
            .Select(g => new
            {
                g.Key.League,
                g.Key.TiglUserId,
                GamesPlayedThisSeason = g.Count(x => x.Season == seasonEnded),
                WinsThisSeason = g.Count(x => x.Season == seasonEnded && x.IsWinner),
                TotalGames = g.Count(),
                TotalWins = g.Count(x => x.IsWinner),
                TotalGamesForActivity = g.Count(x => x.Season == seasonEnded || x.Season == seasonEnded - 1),
            })
            .ToListAsync(cancellationToken);

        // Build fast lookup maps for raw leagues
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
                g => g.ToDictionary(x => x.TiglUserId, x => new { x.GamesPlayedThisSeason, x.WinsThisSeason, x.TotalGames, x.TotalWins, x.TotalGamesForActivity })
            );

        // Standard and Fractured are the only public leaderboards. For Standard we
        // aggregate over ProphecyOfKings (seasons <= 13) or ProphecyOfKings+ThundersEdge (seasons >= 14).
        var publicLeagues = new[] { TiglLeague.ProphecyOfKings, TiglLeague.Fractured };

        foreach (var publicLeague in publicLeagues)
        {
            var rows = new List<PlayerSeasonResult>();

            foreach (var u in allTiglUsers)
            {
                // For standard ladder, map to underlying raw leagues depending on season
                TiglLeague[] sourceLeagues;

                if (publicLeague == TiglLeague.ProphecyOfKings)
                {
                    // Standard ladder: PoK only for <= 13, PoK + TE for >= 14
                    sourceLeagues = seasonEnded <= 13
                        ? new[] { TiglLeague.ProphecyOfKings }
                        : new[] { TiglLeague.ProphecyOfKings, TiglLeague.ThundersEdge };
                }
                else
                {
                    // Fractured stays as is
                    sourceLeagues = new[] { TiglLeague.Fractured };
                }

                // Ratings: for >= 14 use ThundersEdge, otherwise ProphecyOfKings
                var ratingLeague = publicLeague == TiglLeague.ProphecyOfKings && seasonEnded >= 14
                    ? TiglLeague.ThundersEdge
                    : publicLeague;

                var asyncRating = asyncByLeagueUser.TryGetValue(ratingLeague, out var aMap) && aMap.TryGetValue(u.Id, out var aR)
                    ? aR
                    : null;
                var glickoRating = glickoByLeagueUser.TryGetValue(ratingLeague, out var gMap) && gMap.TryGetValue(u.Id, out var gR)
                    ? gR
                    : null;
                var trueskillRating = trueSkillByLeagueUser.TryGetValue(ratingLeague, out var tMap) && tMap.TryGetValue(u.Id, out var tR)
                    ? tR
                    : null;

                int gamesPlayedThisSeason = 0;
                int winsThisSeason = 0;
                int totalGames = 0;
                int totalWins = 0;
                int totalGamesForActivity = 0;

                foreach (var srcLeague in sourceLeagues)
                {
                    if (!aggregatesByLeague.TryGetValue(srcLeague, out var agMap) || !agMap.TryGetValue(u.Id, out var aggr))
                        continue;

                    gamesPlayedThisSeason += aggr.GamesPlayedThisSeason;
                    winsThisSeason += aggr.WinsThisSeason;
                    totalGames += aggr.TotalGames;
                    totalWins += aggr.TotalWins;
                    totalGamesForActivity += aggr.TotalGamesForActivity;
                }

                var row = new PlayerSeasonResult
                {
                    Season = seasonEnded,
                    TiglUserId = u.Id,
                    TiglUserName = u.TiglUserName,
                    TiglDiscordTag = u.DiscordTag,
                    League = publicLeague,
                    GamesPlayed = totalGames, // overall games in standard/fractured
                    WinPercentage = totalGames == 0 ? 0 : Math.Round((totalWins / (double)totalGames) * 100.0, 4),

                    AsyncRating = asyncRating?.Rating ?? 1000.0,
                    AussieScore = asyncRating?.AussieScore ?? 0.0,

                    GlickoRating = glickoRating?.Rating ?? 1500.0,
                    GlickoRd = glickoRating?.Rd ?? 350.0,
                    GlickoVolatility = glickoRating?.Volatility ?? 0.06,

                    TrueSkillMu = trueskillRating?.Rating ?? 25.0,
                    TrueSkillSigma = trueskillRating?.Sigma ?? 8.333,
                    TrueSkillConservativeRating = trueskillRating?.ConservativeRating ?? 0.0,

                    IsActive = totalGamesForActivity > 0,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                };

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
