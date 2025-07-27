using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public class GlickoRatingCalculatorService : IGlickoRatingCalculatorService
{
    private const double Tau = 0.5;
    private const double Epsilon = 0.000001;
    private const double Pi2 = Math.PI * Math.PI;
    private const double RatingScale = 173.7178;
    private const double InitialRating = 1500.00;

    public async Task UpdatePlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, int season)
    {
        var preMatchSnapshot = GetPrematchSnapshot(matchStats);

        // Calculate new ratings based on snapshot
        foreach (var player in matchStats)
        {
            var playerSnapshot = preMatchSnapshot[player.DiscordUserId];
            var results = new List<GlickoMatchResult>();

            foreach (var opponent in matchStats)
            {
                if (player.DiscordUserId == opponent.DiscordUserId)
                    continue;

                var result = GetPlayerResultAgainstOpponent(preMatchSnapshot, player, playerSnapshot, opponent);
                results.Add(result);
            }

            var playerNewRating = CalculateNewPlayerStats(playerSnapshot, results);
            var avgRating = CalculateAverageOpponentRating(preMatchSnapshot, player.DiscordUserId);

            player.RatingNew = playerNewRating.Mu;
            player.RdNew = playerNewRating.Phi;
            player.VolatilityNew = playerNewRating.Sigma;
            player.OpponentAvgRating = avgRating;
            player.Season = season;
        }

        await Task.CompletedTask;
    }

    public Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, TiglLeague league)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(matchStats);

        foreach (var player in players)
        {
            var playerStats = matchStats.FirstOrDefault(x => x.DiscordUserId == player.DiscordId);
            var glickoStats = player.GetCorrectGlickoStats(league);
            var glickoRating = glickoStats?.Rating;

            if (glickoRating is not null && playerStats is not null)
            {
                glickoRating.Rating = playerStats.RatingNew;
                glickoRating.Volatility = playerStats.VolatilityNew;
                glickoRating.Rd = playerStats.RdNew;
            }
        }

        return Task.CompletedTask;
    }

    private static Dictionary<long, GlickoPlayerSnapshot> GetPrematchSnapshot(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats)
    {
        return matchStats.ToDictionary(
            p => p.DiscordUserId,
            p => new GlickoPlayerSnapshot(
                (p.RatingOld - InitialRating) / RatingScale,
                p.RdOld / RatingScale,
                p.VolatilityOld,
                p.RatingOld));
    }

    private static double CalculateAverageOpponentRating(Dictionary<long, GlickoPlayerSnapshot> opponents, long playerDiscordId)
    {
        return opponents.Where(x => x.Key != playerDiscordId).Average(x => x.Value.Rating);
    }

    private static GlickoMatchResult GetPlayerResultAgainstOpponent(Dictionary<long, GlickoPlayerSnapshot> preMatchSnapshot, GlickoPlayerMatchStats player, GlickoPlayerSnapshot playerSnapshot, GlickoPlayerMatchStats opponent)
    {
        var opponentSnapshot = preMatchSnapshot[opponent.DiscordUserId];
        double actualOutcome = player.Performance / (player.Performance + opponent.Performance);
        double opponentImpactFactor = 1.0 / Math.Sqrt(1 + (3 * Math.Pow(opponentSnapshot.Phi, 2) / Pi2));
        double expectedOutcome = 1.0 / (1 + Math.Exp(-opponentImpactFactor * (playerSnapshot.Mu - opponentSnapshot.Mu)));

        return new GlickoMatchResult(opponentImpactFactor, expectedOutcome, actualOutcome);
    }

    private static GlickoPlayerSnapshot CalculateNewPlayerStats(GlickoPlayerSnapshot playerSnapshot, IReadOnlyCollection<GlickoMatchResult> results)
    {
        double inverseOfRatingVariance = results.Sum(r => r.OpponentImpactFactor * r.OpponentImpactFactor * r.ExpectedOutcome * (1 - r.ExpectedOutcome));
        double estimatedVariance = 1.0 / inverseOfRatingVariance;
        double delta = estimatedVariance * results.Sum(r => r.OpponentImpactFactor * (r.ActualOutcome - r.ExpectedOutcome));
        double startingVolatility = Math.Log(playerSnapshot.Sigma * playerSnapshot.Sigma);
        double leftBound = startingVolatility;
        double rightBound;

        double F(double x)
        {
            double expX = Math.Exp(x);
            double num = expX * ((delta * delta) - (playerSnapshot.Phi * playerSnapshot.Phi) - estimatedVariance - expX);
            double denom = 2 * Math.Pow((playerSnapshot.Phi * playerSnapshot.Phi) + estimatedVariance + expX, 2);
            return (num / denom) - ((x - startingVolatility) / (Tau * Tau));
        }

        if (delta * delta > ((playerSnapshot.Phi * playerSnapshot.Phi) + estimatedVariance))
        {
            rightBound = Math.Log((delta * delta) - (playerSnapshot.Phi * playerSnapshot.Phi) - estimatedVariance);
        }
        else
        {
            int k = 1;

            while (F(startingVolatility - (k * Tau)) < 0)
            {
                k++;
            }

            rightBound = startingVolatility - (k * Tau);
        }

        double fA = F(leftBound);
        double fB = F(rightBound);

        while (Math.Abs(rightBound - leftBound) > Epsilon)
        {
            double temp = leftBound + ((leftBound - rightBound) * fA / (fB - fA));
            double fC = F(temp);
            if (fC * fB < 0)
            {
                leftBound = rightBound;
                fA = fB;
            }
            else
            {
                fA /= 2;
            }

            rightBound = temp;
            fB = fC;
        }

        double sigmaPrime = Math.Exp(leftBound / 2);
        double phiStar = Math.Sqrt((playerSnapshot.Phi * playerSnapshot.Phi) + (sigmaPrime * sigmaPrime));
        double phiPrime = 1.0 / Math.Sqrt((1.0 / (phiStar * phiStar)) + (1.0 / estimatedVariance));
        double muPrime = playerSnapshot.Mu + (phiPrime * phiPrime * results.Sum(r => r.OpponentImpactFactor * (r.ActualOutcome - r.ExpectedOutcome)));

        double newMu = (muPrime * RatingScale) + InitialRating;
        double newPhiPrime = phiPrime * RatingScale;

        return new GlickoPlayerSnapshot(newMu, newPhiPrime, sigmaPrime, playerSnapshot.Rating);
    }
}
