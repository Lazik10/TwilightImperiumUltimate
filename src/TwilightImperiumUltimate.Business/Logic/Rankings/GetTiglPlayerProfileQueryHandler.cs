using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetTiglPlayerProfileQueryHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<GetTiglPlayerProfileQuery, TiglPlayerProfileDto>
{
    private static readonly TiglLeague[] Leagues = { TiglLeague.ProphecyOfKings, TiglLeague.ThundersEdge, TiglLeague.Fractured };

    public async Task<TiglPlayerProfileDto> Handle(GetTiglPlayerProfileQuery request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var userId = request.TiglUserId;

        var user = await db.TiglUsers
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, u.TiglUserName, u.DiscordId, u.DiscordTag })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return new TiglPlayerProfileDto { TiglUserId = userId };
        }

        var profile = new TiglPlayerProfileDto
        {
            TiglUserId = user.Id,
            TiglUserName = user.TiglUserName,
            DiscordId = user.DiscordId,
            DiscordTag = user.DiscordTag,
        };

        // Ratings per league
        var asyncStats = await db.AsyncStats
            .Include(s => s.Rating)
            .AsNoTracking()
            .Where(s => s.TiglUserId == userId)
            .ToListAsync(cancellationToken);

        var glickoStats = await db.GlickoStats
            .Include(s => s.Rating)
            .AsNoTracking()
            .Where(s => s.TiglUserId == userId)
            .ToListAsync(cancellationToken);

        var trueSkillStats = await db.TrueSkillStats
            .Include(s => s.TrueSkillRating)
            .AsNoTracking()
            .Where(s => s.TiglUserId == userId)
            .ToListAsync(cancellationToken);

        // Match history per rating system
        var asyncStatsIds = asyncStats.Select(s => s.Id).ToList();
        var asyncMatchHistory = await db.AsyncPlayerMatchStats
            .AsNoTracking()
            .Where(m => asyncStatsIds.Contains(m.AsyncStatsId))
            .OrderBy(m => m.EndTimestamp)
            .ToListAsync(cancellationToken);

        var glickoStatsIds = glickoStats.Select(s => s.Id).ToList();
        var glickoMatchHistory = await db.GlickoPlayerMatchStats
            .AsNoTracking()
            .Where(m => glickoStatsIds.Contains(m.GlickoStatsId))
            .OrderBy(m => m.EndTimestamp)
            .ToListAsync(cancellationToken);

        var trueSkillStatsIds = trueSkillStats.Select(s => s.Id).ToList();
        var trueSkillMatchHistory = await db.TrueSkillPlayerMatchStats
            .AsNoTracking()
            .Where(m => trueSkillStatsIds.Contains(m.TrueSkillStatsId))
            .OrderBy(m => m.EndTimestamp)
            .ToListAsync(cancellationToken);

        // Rank history
        var rankHistory = await db.Ranks
            .AsNoTracking()
            .Where(r => r.TiglUserId == userId)
            .OrderByDescending(r => r.AchievedAt)
            .Select(r => new RankHistoryDto { Id = r.Id, League = r.League, Rank = r.Name, AchievedAt = r.AchievedAt })
            .ToListAsync(cancellationToken);

        profile.RankHistory = rankHistory;

        // Prestige rank history
        var prestigeHistory = await db.TiglUserPrestigeRanks
            .Include(p => p.PrestigeRank)
            .AsNoTracking()
            .Where(p => p.TiglUserId == userId)
            .OrderByDescending(p => p.AchievedAt)
            .Select(p => new PrestigeRankHistoryDto
            {
                League = p.PrestigeRank.League,
                PrestigeRank = p.PrestigeRank.Name,
                Faction = p.PrestigeRank.FactionName,
                Level = p.Rank,
                AchievedAt = p.AchievedAt,
            })
            .ToListAsync(cancellationToken);

        profile.PrestigeRankHistory = prestigeHistory;

        // Achievements
        var achievements = await db.TiglUserAchievements
            .Include(a => a.Achievement)
            .AsNoTracking()
            .Where(a => a.TiglUserId == userId)
            .Select(a => new TiglUserAchievementDto
            {
                TiglUserId = a.TiglUserId,
                AchievementName = a.AchievementName,
                Category = a.Achievement.Category,
                Faction = a.Faction,
                AchievedAt = a.AchievedAt,
                MatchId = a.MatchId,
                MatchName = a.MatchName,
            })
            .ToListAsync(cancellationToken);

        profile.Achievements = achievements;

        // Total users for rarity
        profile.TotalTiglUsers = await db.TiglUsers.CountAsync(cancellationToken);

        // Achievement player counts for rarity
        var achievementCounts = await db.TiglUserAchievements
            .AsNoTracking()
            .GroupBy(a => a.AchievementName)
            .Select(g => new { Name = g.Key, Count = g.Select(a => a.TiglUserId).Distinct().Count() })
            .ToListAsync(cancellationToken);

        profile.AchievementPlayerCounts = achievementCounts.ToDictionary(a => a.Name.ToString(), a => a.Count);

        // Game history for the player
        var gameHistory = await db.GameReports
            .AsNoTracking()
            .Where(m => m.State == MatchState.Evaluated)
            .Where(m => m.PlayerResults.Any(pr => pr.TiglUserId == userId))
            .SelectMany(m => m.PlayerResults
                .Where(pr => pr.TiglUserId == userId)
                .Select(pr => new TiglProfileGameDto
                {
                    MatchReportId = m.Id,
                    GameId = m.GameId,
                    League = m.League,
                    State = m.State,
                    Source = m.Source,
                    StartTimestamp = m.StartTimestamp,
                    EndTimestamp = m.EndTimestamp,
                    MaxScore = m.Score,
                    IsWinner = pr.IsWinner,
                    Faction = pr.Faction,
                    Score = pr.Score,
                    Season = m.Season,
                }))
            .OrderByDescending(g => g.EndTimestamp)
            .ToListAsync(cancellationToken);

        profile.GameHistory = gameHistory;

        // Faction statistics per league
        profile.FactionStats = gameHistory
            .GroupBy(g => new { g.Faction, g.League })
            .Select(g => new TiglProfileFactionStatsDto
            {
                Faction = g.Key.Faction,
                League = g.Key.League,
                Games = g.Count(),
                Wins = g.Count(x => x.IsWinner),
                MinVp = g.Min(x => x.Score),
                MaxVp = g.Max(x => x.Score),
                TotalScoredVp = g.Sum(x => x.Score),
                MaxPossibleVp = g.Sum(x => x.MaxScore),
            })
            .ToList();

        // Top opponents per league
        var matchIdsWithUser = await db.GameReports
            .AsNoTracking()
            .Where(m => m.State == MatchState.Evaluated)
            .Where(m => m.PlayerResults.Any(pr => pr.TiglUserId == userId))
            .Select(m => new { m.Id, m.League })
            .ToListAsync(cancellationToken);

        var matchIdSet = matchIdsWithUser.Select(m => m.Id).ToHashSet();
        var matchLeagueMap = matchIdsWithUser.ToDictionary(m => m.Id, m => m.League);

        var opponentResults = await db.GameReports
            .AsNoTracking()
            .Where(m => matchIdSet.Contains(m.Id))
            .SelectMany(m => m.PlayerResults
                .Where(pr => pr.TiglUserId != userId)
                .Select(pr => new { pr.TiglUserId, m.Id, m.League }))
            .ToListAsync(cancellationToken);

        var opponentCounts = opponentResults
            .GroupBy(x => new { x.TiglUserId, x.League })
            .Select(g => new { g.Key.TiglUserId, g.Key.League, GamesPlayed = g.Count() })
            .ToList();

        var opponentIds = opponentCounts.Select(o => o.TiglUserId).Distinct().ToList();
        var opponentNames = await db.TiglUsers
            .AsNoTracking()
            .Where(u => opponentIds.Contains(u.Id))
            .Select(u => new { u.Id, u.TiglUserName })
            .ToDictionaryAsync(u => u.Id, u => u.TiglUserName, cancellationToken);

        profile.TopOpponents = opponentCounts
            .Select(o => new TiglTopOpponentDto
            {
                TiglUserId = o.TiglUserId,
                TiglUserName = opponentNames.GetValueOrDefault(o.TiglUserId, "Unknown"),
                League = o.League,
                GamesPlayed = o.GamesPlayed,
            })
            .OrderByDescending(o => o.GamesPlayed)
            .ToList();

        // Games played per league
        var gamesPlayed = await db.GameReports
            .AsNoTracking()
            .Where(m => m.State == MatchState.Evaluated)
            .SelectMany(m => m.PlayerResults.Select(pr => new { pr.TiglUserId, m.League }))
            .Where(x => x.TiglUserId == userId)
            .GroupBy(x => x.League)
            .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);

        // Build league profiles
        foreach (var league in Leagues)
        {
            var asyncStat = asyncStats.FirstOrDefault(s => s.League == league);
            var glickoStat = glickoStats.FirstOrDefault(s => s.League == league);
            var trueSkillStat = trueSkillStats.FirstOrDefault(s => s.League == league);

            var leagueRanks = rankHistory.Where(r => r.League == league).ToList();
            var highestRank = leagueRanks.Count > 0
                ? leagueRanks.MaxBy(r => r.Rank)?.Rank ?? TiglRankName.Unranked
                : TiglRankName.Unranked;
            var currentRank = leagueRanks.Count > 0
                ? leagueRanks.OrderByDescending(r => r.AchievedAt).First().Rank
                : TiglRankName.Unranked;

            gamesPlayed.TryGetValue(league, out var games);

            var leagueProfile = new TiglLeagueProfileDto
            {
                League = league,
                AsyncRating = asyncStat?.Rating?.Rating ?? 1000.0,
                AsyncAussieScore = asyncStat?.Rating?.AussieScore ?? 0,
                GlickoRating = glickoStat?.Rating?.Rating ?? 1500.0,
                GlickoRd = glickoStat?.Rating?.Rd ?? 350.0,
                GlickoVolatility = glickoStat?.Rating?.Volatility ?? 0.06,
                TrueSkillMu = trueSkillStat?.TrueSkillRating?.Mu ?? 25.0,
                TrueSkillSigma = trueSkillStat?.TrueSkillRating?.Sigma ?? (25.0 / 3),
                CurrentRank = currentRank,
                HighestRank = highestRank,
                GamesPlayed = games,
            };

            // Map match history
            if (asyncStat is not null)
            {
                leagueProfile.AsyncMatchHistory = asyncMatchHistory
                    .Where(m => m.AsyncStatsId == asyncStat.Id)
                    .Select(m => new AsyncPlayerMatchStatsDto
                    {
                        TiglUserId = userId,
                        DiscordUserId = m.DiscordUserId,
                        Winner = m.Winner,
                        Placement = m.Placement,
                        Score = m.Score,
                        Faction = m.Faction,
                        RatingOld = m.RatingOld,
                        RatingNew = m.RatingNew,
                        AussieScoreOld = m.AussieScoreOld,
                        AussieScoreNew = m.AussieScoreNew,
                        OpponentAvgRating = m.OpponentAvgRating,
                        Season = m.Season,
                        StartTimestamp = m.StartTimestamp,
                        EndTimestamp = m.EndTimestamp,
                        IsRankUpGame = m.IsRankUpGame,
                        ForcedReset = m.ForcedReset,
                        OldRank = m.OldRank,
                        NewRank = m.NewRank,
                    })
                    .ToList();
            }

            if (glickoStat is not null)
            {
                leagueProfile.GlickoMatchHistory = glickoMatchHistory
                    .Where(m => m.GlickoStatsId == glickoStat.Id)
                    .Select(m => new GlickoPlayerMatchStatsDto
                    {
                        TiglUserId = userId,
                        DiscordUserId = m.DiscordUserId,
                        Winner = m.Winner,
                        Placement = m.Placement,
                        Score = m.Score,
                        Faction = m.Faction,
                        RatingOld = m.RatingOld,
                        RatingNew = m.RatingNew,
                        RdOld = m.RdOld,
                        RdNew = m.RdNew,
                        VolatilityOld = m.VolatilityOld,
                        VolatilityNew = m.VolatilityNew,
                        OpponentAvgRating = m.OpponentAvgRating,
                        Season = m.Season,
                        StartTimestamp = m.StartTimestamp,
                        EndTimestamp = m.EndTimestamp,
                        IsRankUpGame = m.IsRankUpGame,
                        ForcedReset = m.ForcedReset,
                        OldRank = m.OldRank,
                        NewRank = m.NewRank,
                    })
                    .ToList();
            }

            if (trueSkillStat is not null)
            {
                leagueProfile.TrueSkillMatchHistory = trueSkillMatchHistory
                    .Where(m => m.TrueSkillStatsId == trueSkillStat.Id)
                    .Select(m => new TrueSkillPlayerMatchStatsDto
                    {
                        TiglUserId = userId,
                        DiscordUserId = m.DiscordUserId,
                        Winner = m.Winner,
                        Placement = m.Placement,
                        Score = m.Score,
                        Faction = m.Faction,
                        MuOld = m.MuOld,
                        MuNew = m.MuNew,
                        SigmaOld = m.SigmaOld,
                        SigmaNew = m.SigmaNew,
                        OpponentAvgRating = m.OpponentAvgRating,
                        Season = m.Season,
                        StartTimestamp = m.StartTimestamp,
                        EndTimestamp = m.EndTimestamp,
                        IsRankUpGame = m.IsRankUpGame,
                        ForcedReset = m.ForcedReset,
                        OldRank = m.OldRank,
                        NewRank = m.NewRank,
                    })
                    .ToList();
            }

            profile.LeagueProfiles.Add(leagueProfile);
        }

        return profile;
    }
}
