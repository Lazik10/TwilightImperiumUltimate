using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public class AsyncRatingCalculatorService : IAsyncRatingCalculatorService
{
    private const int K = 32;
    private const double DValue = 400.0;
    private const double DeltaMultiplier = 5.0;
    private const double ExponentialBase = 2.0;
    private const double ExponentialDenominator = 57.0;

    public Task UpdatePlayerMatchStats(IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, int season)
    {
        ArgumentNullException.ThrowIfNull(matchStats);

        var resultSnapshot = matchStats.ToList();

        foreach (var player in matchStats)
        {
            player.ExpectedWinPercentage = CalculateExpectedWinPercentage(player, resultSnapshot);
            player.RatingNew = Math.Round(player.RatingOld + CalculateRatingDelta(player.ExpectedWinPercentage, player.Placement, matchStats.Count), 2);
            player.AussieScoreNew += player.AussieScoreOld + CalculateAussieScore(player, resultSnapshot);
            player.OpponentAvgRating = CalculateAverageRating(matchStats, player.DiscordUserId);
            player.Season = season;
        }

        return Task.CompletedTask;
    }

    public Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(matchStats);

        foreach (var player in players)
        {
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);
            var asyncStats = player.GetCorrectAsyncStats(league);
            var asyncRating = asyncStats?.Rating;

            if (asyncRating is not null && playerStats is not null)
            {
                asyncRating.Rating = playerStats.RatingNew;
                asyncRating.AussieScore = playerStats.AussieScoreNew;
            }
        }

        return Task.CompletedTask;
    }

    private static double CalculateAverageRating(IEnumerable<AsyncPlayerMatchStats> opponents, long playerDiscordId)
    {
        return opponents.Where(x => x.DiscordUserId != playerDiscordId).Average(x => x.RatingOld);
    }

    private static double CalculateExpectedWinPercentage(AsyncPlayerMatchStats player, List<AsyncPlayerMatchStats> allPlayers)
    {
        double sum = 0.0;
        foreach (var opponent in allPlayers)
        {
            if (opponent == player)
                continue;
            sum += 1.0 / (1.0 + Math.Pow(10.0, (opponent.RatingOld - player.RatingOld) / DValue));
        }

        return sum / 15.0;
    }

    private static double CalculateRatingDelta(double expected, int rank, int playerCount)
    {
        double performance = (Math.Pow(ExponentialBase, playerCount - rank) - 1) / ExponentialDenominator;
        double delta = DeltaMultiplier * K * (performance - expected);
        return Math.Round(delta, 2);
    }

    private static double CalculateAussieScore(AsyncPlayerMatchStats player, List<AsyncPlayerMatchStats> opponents)
    {
        int totalVP = opponents.Sum(x => x.Score);
        int maxVP = opponents.Max(x => x.Score);

        if (totalVP == 0)
            return 0;

        int isWinner = player.Score == maxVP ? 1 : 0;
        double share = (player.Score * (double)opponents.Count) / totalVP;
        double aussieScore = isWinner + share;

        return Math.Round(aussieScore, 2);
    }
}
