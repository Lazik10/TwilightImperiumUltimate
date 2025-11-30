using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.DataAccess.DTOs;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class RankingsRepository(
    IDbContextFactory<TwilightImperiumDbContext> contextFactory)
    : IRankingsRepository
{
    public async Task<List<RankingsRow>> GetUsersRankingsOverview(CancellationToken cancellationToken)
    {
        using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

        // Basic user list
        var users = await db.TiglUsers
            .AsNoTracking()
            .Select(u => new { u.Id, u.TiglUserName })
            .ToListAsync(cancellationToken);

        var leagues = new[] { TiglLeague.ProphecyOfKings, TiglLeague.ThundersEdge, TiglLeague.Fractured };

        // Load all normal rank records minimally; we'll pick latest per (user, league)
        var rankRows = await db.Ranks
            .AsNoTracking()
            .Select(r => new { r.TiglUserId, r.League, r.AchievedAt, r.Name })
            .ToListAsync(cancellationToken);

        var lastRankByUserLeague = rankRows
            .GroupBy(r => new { r.TiglUserId, r.League })
            .ToDictionary(
                g => (g.Key.TiglUserId, g.Key.League),
                g => g.OrderByDescending(x => x.AchievedAt).First());

        // Load prestige ranks; we will:
        // - compute last allowed prestige per league
        // - compute faction prestige count per (user, league)
        var prestigeRows = await db.TiglUserPrestigeRanks
            .Include(p => p.PrestigeRank)
            .AsNoTracking()
            .Select(p => new
            {
                p.TiglUserId,
                p.AchievedAt,
                League = p.PrestigeRank.League,
                PrestigeName = p.PrestigeRank.Name,
                Level = p.Rank,
                IsFaction = p.PrestigeRank.FactionName != TiglFactionName.None,
            })
            .ToListAsync(cancellationToken);

        bool AllowedPrestigeForLeague(TiglLeague l, TiglPrestigeRank name) => l switch
        {
            TiglLeague.ProphecyOfKings => name is TiglPrestigeRank.PaxMagnificaBellumGloriosum or TiglPrestigeRank.GalacticThreat,
            TiglLeague.ThundersEdge => name is TiglPrestigeRank.PaxMagnificaBellumGloriosum or TiglPrestigeRank.GalacticThreat,
            TiglLeague.Fractured => name is TiglPrestigeRank.Tyrant,
            _ => false,
        };

        var lastPrestigeByUserLeague = prestigeRows
            .Where(p => AllowedPrestigeForLeague(p.League, p.PrestigeName))
            .GroupBy(p => new { p.TiglUserId, p.League })
            .ToDictionary(
                g => (g.Key.TiglUserId, g.Key.League),
                g => g.OrderByDescending(x => x.AchievedAt).First());

        var factionPrestigeCountByUserLeague = prestigeRows
            .Where(p => p.IsFaction)
            .GroupBy(p => new { p.TiglUserId, p.League })
            .ToDictionary(
                g => (g.Key.TiglUserId, g.Key.League),
                g => g.Count());

        // Games played per user per league from GameReports
        var gamesPlayedByUserLeague = await db.GameReports
            .AsNoTracking()
            .SelectMany(m => m.PlayerResults.Select(pr => new { pr.TiglUserId, m.League }))
            .GroupBy(x => new { x.TiglUserId, x.League })
            .ToDictionaryAsync(g => (g.Key.TiglUserId, g.Key.League), g => g.Count(), cancellationToken);

        // Build result rows for each user and league
        var rows = new List<RankingsRow>();

        foreach (var user in users)
        {
            foreach (var league in leagues)
            {
                lastRankByUserLeague.TryGetValue((user.Id, league), out var rank);
                lastPrestigeByUserLeague.TryGetValue((user.Id, league), out var prestige);
                factionPrestigeCountByUserLeague.TryGetValue((user.Id, league), out var factionCount);
                gamesPlayedByUserLeague.TryGetValue((user.Id, league), out var gamesPlayed);

                var lastAchieved = Math.Max(rank?.AchievedAt ?? 0, prestige?.AchievedAt ?? 0);

                rows.Add(new RankingsRow
                {
                    TiglUserId = user.Id,
                    TiglUserName = user.TiglUserName,
                    League = league,
                    HighestRank = rank?.Name ?? TiglRankName.Unranked,
                    LastAchievedAt = lastAchieved,
                    HighestPrestigeRank = prestige?.PrestigeName,
                    HighestPrestigeRankLevel = prestige?.Level ?? 0,
                    FactionPrestigeRankCount = factionCount,
                    GamesPlayed = gamesPlayed,
                });
            }
        }

        return rows;
    }

    public async Task<List<RankingsLeaderDto>> GetLeadersOverview(CancellationToken cancellationToken)
    {
        using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

        var leaders = await db.Leaders
            .Include(x => x.CurrentOwner)
            .AsNoTracking()
            .Select(l => new RankingsLeaderDto
            {
                Name = l.Name,
                Faction = l.Faction,
                League = l.League,
                UserName = l.CurrentOwner != null ? l.CurrentOwner.TiglUserName : string.Empty,
                LastUpdate = l.LastUpdate,
                ChangeCount = l.ChangeCount,
                ShortestDuration = l.ShortestDuration,
                LongestDuration = l.LongestDuration,
            })
            .ToListAsync(cancellationToken);

        return leaders;
    }
}
