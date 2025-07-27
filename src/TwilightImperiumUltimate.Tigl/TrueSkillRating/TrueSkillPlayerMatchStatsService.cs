using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public class TrueSkillPlayerMatchStatsService : ITrueSkillPlayerMatchStatsService
{
    public Task<List<TrueSkillPlayerMatchStats>> InitializePlayerMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(report);

        var playerMatchStats = CreateMatchStats(report, gameId, players, league);

        UpdatePlayerPlacements(playerMatchStats);
        AssignWinner(playerMatchStats);

        return Task.FromResult(playerMatchStats);
    }

    private static List<TrueSkillPlayerMatchStats> CreateMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league)
    {
        return [.. report.PlayerResults
            .OrderByDescending(s => s.Score)
            .Select(x =>
            {
                var tiglUser = players.FirstOrDefault(p => p.DiscordId == x.DiscordId);
                var trueSkillStats = tiglUser?.TrueSkillStats?.FirstOrDefault(x => x.League == league);

                return new TrueSkillPlayerMatchStats
                {
                    TrueSkillStatsId = trueSkillStats?.Id ?? 0,
                    MatchId = gameId,
                    DiscordUserId = x.DiscordId,
                    Score = Math.Min(x.Score, report.Score),
                    MuOld = trueSkillStats?.TrueSkillRating?.Mu ?? 0,
                    SigmaOld = trueSkillStats?.TrueSkillRating?.Sigma ?? 0,
                    Faction = TiglFactionParser.ParseFaction(x.Faction),
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };
            })];
    }

    private static void UpdatePlayerPlacements(List<TrueSkillPlayerMatchStats> trueSkillPlayerMatchStats)
    {
        int currentPlacement = 1;
        int i = 0;

        while (i < trueSkillPlayerMatchStats.Count)
        {
            var currentScore = trueSkillPlayerMatchStats[i].Score;
            int tiedCount = trueSkillPlayerMatchStats.Count(s => s.Score == currentScore);

            for (int j = 0; j < tiedCount; j++)
            {
                trueSkillPlayerMatchStats[i + j].Placement = currentPlacement;
            }

            i += tiedCount;
            currentPlacement += tiedCount;
        }
    }

    private static void AssignWinner(List<TrueSkillPlayerMatchStats> playerMatchStats) => playerMatchStats[0].Winner = true;
}
