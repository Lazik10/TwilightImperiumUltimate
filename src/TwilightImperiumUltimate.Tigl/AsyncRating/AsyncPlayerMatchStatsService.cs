using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public class AsyncPlayerMatchStatsService : IAsyncPlayerMatchStatsService
{
    public Task<List<AsyncPlayerMatchStats>> InitializePlayerMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(report);

        var playerMatchStats = CreateMatchStats(report, gameId, players, league);

        UpdatePlayerPlacements(playerMatchStats);
        AssignWinner(playerMatchStats);

        return Task.FromResult(playerMatchStats);
    }

    private static List<AsyncPlayerMatchStats> CreateMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        return [.. report.PlayerResults
            .OrderByDescending(s => s.Score)
            .Select(x =>
            {
                var tiglUser = players.FirstOrDefault(p => p.DiscordId == x.DiscordId);
                var asyncStats = tiglUser?.AsyncStats?.FirstOrDefault(x => x.League == league);

                return new AsyncPlayerMatchStats
                {
                    AsyncStatsId = asyncStats?.Id ?? 0,
                    MatchId = gameId,
                    DiscordUserId = x.DiscordId,
                    Score = Math.Min(x.Score, report.Score),
                    RatingOld = asyncStats?.Rating?.Rating ?? 0,
                    AussieScoreOld = asyncStats?.Rating?.AussieScore ?? 0,
                    Faction = TiglFactionParser.ParseFaction(x.Faction),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };
            })];
    }

    private static void UpdatePlayerPlacements(List<AsyncPlayerMatchStats> asyncPlayerMatchStats)
    {
        int currentPlacement = 1;
        int i = 0;

        while (i < asyncPlayerMatchStats.Count)
        {
            var currentScore = asyncPlayerMatchStats[i].Score;
            int tiedCount = asyncPlayerMatchStats.Count(s => s.Score == currentScore);

            for (int j = 0; j < tiedCount; j++)
            {
                asyncPlayerMatchStats[i + j].Placement = currentPlacement;
            }

            i += tiedCount;
            currentPlacement += tiedCount;
        }
    }

    private static void AssignWinner(List<AsyncPlayerMatchStats> playerMatchStats) => playerMatchStats[0].Winner = true;
}
