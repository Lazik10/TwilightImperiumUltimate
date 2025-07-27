using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public class TrueSkillRatingCalculatorService : ITrueSkillRatingCalculatorService
{
    // Performance variance
    private const double Beta = 4.1667;

    // Dynamics factor
    private const double Tau = 0.083333;

    public Task UpdatePlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, int season)
    {
        ArgumentNullException.ThrowIfNull(matchStats);

        int count = matchStats.Count;

        CalculatePerformance(matchStats);
        var resultSnapshot = matchStats.ToList();

        for (int i = 0; i < count; i++)
        {
            var player = matchStats.ElementAt(i);
            double deltaMu = 0;
            double deltaSigma = 0;

            for (int j = 0; j < count; j++)
            {
                if (i == j) continue;
                var opponent = resultSnapshot[j];
                double c = Math.Sqrt((2 * Beta * Beta) + (player.SigmaOld * player.SigmaOld) + (opponent.SigmaOld * opponent.SigmaOld));
                double expectedScore = 1.0 / (1.0 + Math.Exp((opponent.MuOld - player.MuOld) / c));

                double performanceA = player.Performance;
                double performanceB = opponent.Performance;
                double normalizedOutcome = performanceA / (performanceA + performanceB);

                double scoreDiff = normalizedOutcome - expectedScore;

                deltaMu += scoreDiff * (Beta * Beta / c);
                deltaSigma += (scoreDiff * scoreDiff) * (Beta * Beta / (c * c));
            }

            player.MuNew = player.MuOld + (deltaMu / (count - 1));
            player.SigmaNew = Math.Sqrt(Math.Max(0.0001, (player.SigmaOld * player.SigmaOld) + (Tau * Tau) - (deltaSigma / (count - 1))));

            player.OpponentAvgRating = CalculateAverageRating(matchStats, player.DiscordUserId);
            player.Season = season;
        }

        return Task.CompletedTask;
    }

    public Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(matchStats);

        foreach (var player in players)
        {
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);
            var trueSkillStats = player.GetCorrectTrueSkillStats(league);
            var trueSkillRating = trueSkillStats?.TrueSkillRating;

            if (trueSkillRating is not null && playerStats is not null)
            {
                trueSkillRating.Mu = playerStats.MuNew;
                trueSkillRating.Sigma = playerStats.SigmaNew;
            }
        }

        return Task.CompletedTask;
    }

    private static void CalculatePerformance(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats)
    {
        var maxScore = matchStats.Max(s => s.Score);
        var scores = matchStats.Select(s => (double)s.Score).ToArray();
        var mean = scores.Average();
        var stdDev = Math.Sqrt(scores.Select(s => Math.Pow(s - mean, 2)).Average());
        var maxStdDev = maxScore / 2.0;
        var closeness = 1.0 - Math.Min(stdDev / maxStdDev, 1.0);

        foreach (var player in matchStats)
        {
            if (player.Score == maxScore)
            {
                player.Performance = 0.9 + (0.1 * (1.0 - closeness));
            }
            else
            {
                var basePerformance = Math.Pow((double)player.Score / maxScore, 2);
                player.Performance = basePerformance * (0.7 + (0.3 * closeness));
            }
        }
    }

    private static double CalculateAverageRating(IEnumerable<TrueSkillPlayerMatchStats> opponents, long playerDiscordId)
    {
        return opponents.Where(x => x.DiscordUserId != playerDiscordId).Average(x => x.MuOld);
    }
}
