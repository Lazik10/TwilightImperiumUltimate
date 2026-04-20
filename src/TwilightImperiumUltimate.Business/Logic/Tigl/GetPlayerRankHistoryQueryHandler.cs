using System.Globalization;
using TwilightImperiumUltimate.Business.Helpers;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetPlayerRankHistoryQueryHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<GetPlayerRankHistoryQuery, ItemListDto<TiglPlayerRankHistoryDto>>
{
    private static readonly TiglLeague[] LeagueOrder =
    [
        TiglLeague.ThundersEdge,
        TiglLeague.Fractured,
        TiglLeague.ProphecyOfKings,
    ];

    public async Task<ItemListDto<TiglPlayerRankHistoryDto>> Handle(GetPlayerRankHistoryQuery request, CancellationToken cancellationToken)
    {
        var requestedDiscordIds = request.DiscordUserIds
            .Distinct()
            .Where(id => id > 0)
            .ToList();

        if (requestedDiscordIds.Count == 0)
            return new ItemListDto<TiglPlayerRankHistoryDto>(new List<TiglPlayerRankHistoryDto>());

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var users = await db.TiglUsers
            .AsNoTracking()
            .Where(user => requestedDiscordIds.Contains(user.DiscordId))
            .Select(user => new { user.Id, user.DiscordId })
            .ToListAsync(cancellationToken);

        var requestedTiglUserIds = users
            .Select(user => user.Id)
            .Distinct()
            .ToList();

        if (requestedTiglUserIds.Count == 0)
        {
            var emptyResult = requestedDiscordIds
                .Select(discordUserId => new TiglPlayerRankHistoryDto
                {
                    DiscordUserId = discordUserId,
                    Ranks = new List<TiglPlayerRankHistoryEntryDto>(),
                })
                .ToList();

            return new ItemListDto<TiglPlayerRankHistoryDto>(emptyResult);
        }

        var discordIdByTiglUserId = users
            .GroupBy(user => user.Id)
            .ToDictionary(
                group => group.Key,
                group => group.Select(user => user.DiscordId).First());

        var userCurrentRankRows = await db.TiglUsers
            .AsNoTracking()
            .Where(user => requestedTiglUserIds.Contains(user.Id))
            .Select(user => new
            {
                user.Id,
                user.ThundersEdgeRank,
                user.ShatteredRank,
                user.ProphecyOfKingsRank,
            })
            .ToListAsync(cancellationToken);

        var standardPrestigeRows = await db.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AsNoTracking()
            .Where(x => requestedTiglUserIds.Contains(x.TiglUserId)
                && (x.PrestigeRank.League == TiglLeague.ThundersEdge || x.PrestigeRank.League == TiglLeague.ProphecyOfKings)
                && (x.PrestigeRank.Name == TiglPrestigeRank.GalacticThreat || x.PrestigeRank.Name == TiglPrestigeRank.PaxMagnificaBellumGloriosum))
            .Select(x => new
            {
                x.TiglUserId,
                League = x.PrestigeRank.League,
                Prestige = x.PrestigeRank.Name,
                x.Rank,
                x.AchievedAt,
            })
            .ToListAsync(cancellationToken);

        var fracturedPrestigeRows = await db.TiglUserPrestigeRanks
            .Include(x => x.PrestigeRank)
            .AsNoTracking()
            .Where(x => requestedTiglUserIds.Contains(x.TiglUserId)
                && x.PrestigeRank.League == TiglLeague.Fractured
                && x.PrestigeRank.Name == TiglPrestigeRank.Tyrant)
            .Select(x => new
            {
                x.TiglUserId,
                League = x.PrestigeRank.League,
                Prestige = x.PrestigeRank.Name,
                x.Rank,
                x.AchievedAt,
            })
            .ToListAsync(cancellationToken);

        var highestStandardPrestigeByUserLeague = standardPrestigeRows
            .GroupBy(x => new { x.TiglUserId, x.League })
            .ToDictionary(
                g => (g.Key.TiglUserId, g.Key.League),
                g => g.OrderByDescending(x => x.Prestige == TiglPrestigeRank.PaxMagnificaBellumGloriosum)
                    .ThenByDescending(x => x.Rank)
                    .ThenByDescending(x => x.AchievedAt)
                    .First());

        var highestFracturedPrestigeByUser = fracturedPrestigeRows
            .GroupBy(x => x.TiglUserId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(x => x.Rank)
                    .ThenByDescending(x => x.AchievedAt)
                    .First());

        var userCurrentRanks = userCurrentRankRows
            .ToDictionary(
                user => user.Id,
                user =>
                {
                    highestStandardPrestigeByUserLeague.TryGetValue((user.Id, TiglLeague.ThundersEdge), out var standardPrestige);
                    highestStandardPrestigeByUserLeague.TryGetValue((user.Id, TiglLeague.ProphecyOfKings), out var legacyPrestige);
                    highestFracturedPrestigeByUser.TryGetValue(user.Id, out var fracturedPrestige);

                    return new TiglPlayerCurrentRanksDto
                    {
                        Standard = TiglCurrentRankFormatter.FormatLegacyOrStandard(user.ThundersEdgeRank, standardPrestige?.Prestige, standardPrestige?.Rank),
                        Fractured = TiglCurrentRankFormatter.FormatFractured(user.ShatteredRank, fracturedPrestige?.Prestige, fracturedPrestige?.Rank),
                        Legacy = TiglCurrentRankFormatter.FormatLegacyOrStandard(user.ProphecyOfKingsRank, legacyPrestige?.Prestige, legacyPrestige?.Rank),
                    };
                });

        var rankRows = await db.RankUpLogs
            .AsNoTracking()
            .Where(log => log.TiglUserId.HasValue && requestedTiglUserIds.Contains(log.TiglUserId.Value))
            .Select(log => new
            {
                TiglUserId = log.TiglUserId!.Value,
                AchievedAt = log.Timestamp,
                RankName = log.NewRank,
                log.League,
                log.Duration,
                log.MatchId,
            })
            .ToListAsync(cancellationToken);

        var fallbackRankRows = await db.Ranks
            .AsNoTracking()
            .Where(rank => requestedTiglUserIds.Contains(rank.TiglUserId))
            .Select(rank => new
            {
                rank.TiglUserId,
                AchievedAt = rank.AchievedAt,
                RankName = rank.Name,
                rank.League,
            })
            .ToListAsync(cancellationToken);

        var firstGamesByUserLeague = await db.GameReports
            .AsNoTracking()
            .Where(match => match.Season >= 14)
            .SelectMany(match => match.PlayerResults
                .Where(player => requestedTiglUserIds.Contains(player.TiglUserId))
                .Select(player => new
                {
                    player.TiglUserId,
                    match.League,
                    match.StartTimestamp,
                }))
            .GroupBy(x => new { x.TiglUserId, x.League })
            .Select(g => new
            {
                g.Key.TiglUserId,
                g.Key.League,
                FirstStartTimestamp = g.Min(x => x.StartTimestamp),
            })
            .ToListAsync(cancellationToken);

        var firstGameStartByUserLeague = firstGamesByUserLeague
            .ToDictionary(x => (x.TiglUserId, x.League), x => x.FirstStartTimestamp);

        var matchIds = rankRows.Select(x => x.MatchId).Distinct().ToList();

        var gameRows = await db.GameReports
            .AsNoTracking()
            .Where(match => matchIds.Contains(match.Id))
            .Select(match => new { match.Id, match.GameId })
            .ToListAsync(cancellationToken);

        var gameIdByMatchId = gameRows.ToDictionary(x => x.Id, x => x.GameId);

        var rankLogKeySet = rankRows
            .Select(x => (x.TiglUserId, x.League, x.RankName, x.AchievedAt))
            .ToHashSet();

        var fallbackGameRows = await db.GameReports
            .AsNoTracking()
            .SelectMany(match => match.PlayerResults
                .Where(player => requestedTiglUserIds.Contains(player.TiglUserId))
                .Select(player => new
                {
                    player.TiglUserId,
                    match.League,
                    AchievedAt = match.EndTimestamp,
                    match.GameId,
                    match.Id,
                }))
            .ToListAsync(cancellationToken);

        var fallbackGameByUserLeagueTimestamp = fallbackGameRows
            .GroupBy(x => new { x.TiglUserId, x.League, x.AchievedAt })
            .ToDictionary(
                g => (g.Key.TiglUserId, g.Key.League, g.Key.AchievedAt),
                g => g.OrderBy(x => x.Id).First());

        var fallbackRows = fallbackRankRows
            .Where(x => !rankLogKeySet.Contains((x.TiglUserId, x.League, x.RankName, x.AchievedAt)))
            .Select(x =>
            {
                var hasGame = fallbackGameByUserLeagueTimestamp.TryGetValue((x.TiglUserId, x.League, x.AchievedAt), out var game);
                return new
                {
                    x.TiglUserId,
                    x.AchievedAt,
                    x.RankName,
                    x.League,
                    Duration = 0L,
                    MatchId = hasGame ? game!.Id : 0,
                    GameId = hasGame ? game!.GameId : string.Empty,
                };
            })
            .ToList();

        var combinedRows = rankRows
            .Select(x => new
            {
                x.TiglUserId,
                x.AchievedAt,
                x.RankName,
                x.League,
                x.Duration,
                x.MatchId,
                GameId = gameIdByMatchId.TryGetValue(x.MatchId, out var gameId) ? gameId : string.Empty,
            })
            .Concat(fallbackRows)
            .OrderBy(x => Array.IndexOf(LeagueOrder, x.League))
            .ThenBy(x => x.AchievedAt)
            .ToList();

        var rowsByUser = combinedRows
            .GroupBy(x => x.TiglUserId)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(x => x.League)
                    .OrderBy(leagueGroup => Array.IndexOf(LeagueOrder, leagueGroup.Key))
                    .SelectMany(leagueGroup =>
                    {
                        var leagueRows = leagueGroup
                            .OrderBy(x => x.AchievedAt)
                            .ToList();

                        return leagueRows.Select((x, index) =>
                        {
                            var duration = x.Duration;

                            if (x.League is TiglLeague.ThundersEdge or TiglLeague.Fractured)
                            {
                                var previous = index > 0 ? leagueRows[index - 1] : null;
                                var isFirstBasePromotion =
                                    previous is not null
                                    && previous.RankName == TiglRankName.Unranked
                                    && ((x.League == TiglLeague.ThundersEdge && x.RankName == TiglRankName.Minister)
                                        || (x.League == TiglLeague.Fractured && x.RankName == TiglRankName.Thrall));

                                if (isFirstBasePromotion)
                                {
                                    var startTimestamp = previous!.AchievedAt;
                                    if (firstGameStartByUserLeague.TryGetValue((x.TiglUserId, x.League), out var firstStart)
                                        && firstStart > startTimestamp)
                                    {
                                        startTimestamp = firstStart;
                                    }

                                    duration = Math.Max(0, x.AchievedAt - startTimestamp);
                                }
                                else if (duration <= 0 && previous is not null)
                                {
                                    duration = Math.Max(0, x.AchievedAt - previous.AchievedAt);
                                }
                            }

                            var durationDays = (int)Math.Floor(TimeSpan.FromMilliseconds(duration).TotalDays);

                            return new TiglPlayerRankHistoryEntryDto
                            {
                                Date = DateTimeOffset.FromUnixTimeMilliseconds(x.AchievedAt).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                League = MapLeagueName(x.League),
                                RankName = x.RankName,
                                Duration = Math.Max(0, durationDays),
                                GameId = x.GameId,
                            };
                        });
                    })
                    .ToList());

        var rowsByDiscordUser = rowsByUser
            .Where(x => discordIdByTiglUserId.ContainsKey(x.Key))
            .GroupBy(x => discordIdByTiglUserId[x.Key])
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(x => x.Value)
                    .OrderBy(x => GetLeagueOrder(x.League))
                    .ThenBy(x => x.Date)
                    .ToList());

        var result = requestedDiscordIds
            .Select(discordUserId => new TiglPlayerRankHistoryDto
            {
                DiscordUserId = discordUserId,
                CurrentRanks = users
                    .Where(user => user.DiscordId == discordUserId)
                    .Select(user => userCurrentRanks.TryGetValue(user.Id, out var ranks)
                        ? ranks
                        : new TiglPlayerCurrentRanksDto())
                    .FirstOrDefault() ?? new TiglPlayerCurrentRanksDto(),
                Ranks = rowsByDiscordUser.TryGetValue(discordUserId, out var ranks)
                    ? ranks
                    : new List<TiglPlayerRankHistoryEntryDto>(),
            })
            .ToList();

        return new ItemListDto<TiglPlayerRankHistoryDto>(result);
    }

    private static string MapLeagueName(TiglLeague league) => league switch
    {
        TiglLeague.ThundersEdge => "Standard",
        TiglLeague.Fractured => "Fractured",
        TiglLeague.ProphecyOfKings => "Legacy",
        _ => league.ToString(),
    };

    private static int GetLeagueOrder(string league) => league switch
    {
        "Standard" => 0,
        "Fractured" => 1,
        "Legacy" => 2,
        _ => 99,
    };
}
