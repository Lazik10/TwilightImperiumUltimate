using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public class GlickoPlayerMatchStatsService : IGlickoPlayerMatchStatsService
{
    public Task<List<GlickoPlayerMatchStats>> InitializePlayerMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(report);

        var playerMatchStats = CreateMatchStats(report, gameId, players, league);

        UpdatePlayerPlacements(playerMatchStats);
        AssignWinner(playerMatchStats);

        return Task.FromResult(playerMatchStats);
    }

    private static List<GlickoPlayerMatchStats> CreateMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        return [.. report.PlayerResults
            .OrderByDescending(s => s.Score)
            .Select(x =>
            {
                var tiglUser = players.FirstOrDefault(p => p.DiscordId == x.DiscordId);
                var glickoStats = tiglUser?.GlickoStats?.FirstOrDefault(x => x.League == league);

                return new GlickoPlayerMatchStats
                {
                    GlickoStatsId = glickoStats?.Id ?? 0,
                    MatchId = gameId,
                    DiscordUserId = x.DiscordId,
                    Score = Math.Min(x.Score, report.Score),
                    RatingOld = glickoStats?.Rating?.Rating ?? 0,
                    RdOld = glickoStats?.Rating?.Rd ?? 0,
                    VolatilityOld = glickoStats?.Rating?.Volatility ?? 0,
                    Faction = TiglFactionParser.ParseFaction(x.Faction),
                    StartTimestamp = report.StartTimestamp,
                    EndTimestamp = report.EndTimestamp,
                };
            })];
    }

    private static void UpdatePlayerPlacements(List<GlickoPlayerMatchStats> playerMatchStats)
    {
        int currentPlacement = 1;
        int i = 0;

        while (i < playerMatchStats.Count)
        {
            var currentScore = playerMatchStats[i].Score;
            int tiedCount = playerMatchStats.Count(s => s.Score == currentScore);

            for (int j = 0; j < tiedCount; j++)
            {
                playerMatchStats[i + j].Placement = currentPlacement;
            }

            i += tiedCount;
            currentPlacement += tiedCount;
        }
    }

    private static void AssignWinner(List<GlickoPlayerMatchStats> playerMatchStats) => playerMatchStats[0].Winner = true;
}
