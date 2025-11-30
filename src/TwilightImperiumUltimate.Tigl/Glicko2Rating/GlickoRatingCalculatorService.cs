using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public class GlickoRatingCalculatorService : IGlickoRatingCalculatorService
{
    private const double Scale = 173.7178;
    private const double InitialRating = 1500.0;
    private const double Tau = 0.5;
    private const double MinRD = 30.0;
    private const double MaxRD = 350.0;
    private const int MaxVolatilityIterations = 50;
    private const double VolatilityTolerance = 1e-6;

    // Stability rails (engage only in adversarial/noisy cases)
    private const double VInvFloor = 1e-6;            // avoid v = 1/0 => ∞
    private const double SigmaMin = 1e-12;            // avoid log(0)
    private const double SigmaGrowthCap = 2.0;        // max × growth of sigma per update
    private static readonly double PhiPrimeCap = MaxRD / Scale; // keep update consistent with stored RD cap

    public async Task UpdatePlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, int season)
    {
        if (matchStats is null || matchStats.Count < 2)
            throw new ArgumentException("At least two players are required to compute Glicko-2 updates.", nameof(matchStats));

        var transformed = matchStats.ToDictionary(
            p => p.DiscordUserId,
            p =>
            {
                var mu = (p.RatingOld - InitialRating) / Scale;
                var phi = p.RdOld / Scale;
                var sigma = p.VolatilityOld;
                return (mu, phi, sigma, p.RatingOld);
            });

        var preAvgRatings = matchStats.ToDictionary(
            p => p.DiscordUserId,
            p => matchStats.Where(x => x.DiscordUserId != p.DiscordUserId).Average(x => x.RatingOld));

        foreach (var player in matchStats)
        {
            var (mu_i, phi_i, sigma_i, _) = transformed[player.DiscordUserId];

            var opponents = new List<(double mu_j, double phi_j, double s_ij)>(matchStats.Count - 1);
            foreach (var opp in matchStats)
            {
                if (opp.DiscordUserId == player.DiscordUserId) continue;

                double s = player.Placement < opp.Placement ? 1.0 : player.Placement > opp.Placement ? 0.0 : 0.5;

                var (mu_j, phi_j, _, _) = transformed[opp.DiscordUserId];
                opponents.Add((mu_j, phi_j, s));
            }

            if (opponents.Count == 0)
            {
                player.RatingNew = player.RatingOld;
                player.RdNew = player.RdOld;
                player.VolatilityNew = player.VolatilityOld;
                player.OpponentAvgRating = preAvgRatings[player.DiscordUserId];
                player.Season = season;
                continue;
            }

            double vInv = 0.0;
            double sumDeltaTerm = 0.0;

            foreach (var (mu_j, phi_j, s_ij) in opponents)
            {
                double g = G(phi_j);
                double E = EFunc(mu_i, mu_j, phi_j);
                vInv += g * g * E * (1.0 - E);
                sumDeltaTerm += g * (s_ij - E);
            }

            // Guard against vInv -> 0 to avoid v -> ∞
            if (vInv < VInvFloor) vInv = VInvFloor;

            double v = 1.0 / vInv;
            double delta = v * sumDeltaTerm;

            double sigmaPrime = SolveVolatility(sigma_i, phi_i, delta, v);
            if (!double.IsFinite(sigmaPrime) || sigmaPrime <= 0) sigmaPrime = sigma_i;
            // Limit per-update growth to prevent runaway explosion on adversarial data
            double sigmaMaxThisUpdate = Math.Max(SigmaMin, sigma_i * SigmaGrowthCap);
            if (sigmaPrime > sigmaMaxThisUpdate) sigmaPrime = sigmaMaxThisUpdate;
            if (sigmaPrime < SigmaMin) sigmaPrime = SigmaMin;

            double phiStar = Math.Sqrt((phi_i * phi_i) + (sigmaPrime * sigmaPrime));
            double phiPrime = 1.0 / Math.Sqrt((1.0 / (phiStar * phiStar)) + (1.0 / v));

            // Use a capped phi' for the rating update to keep steps bounded and consistent with stored RD cap
            double phiPrimeForUpdate = Math.Min(phiPrime, PhiPrimeCap);

            double muPrime = mu_i + ((phiPrimeForUpdate * phiPrimeForUpdate) * sumDeltaTerm);

            double rPrime = InitialRating + (muPrime * Scale);
            double rdPrime = Math.Clamp(phiPrime * Scale, MinRD, MaxRD);

            player.RatingNew = rPrime;
            player.RdNew = rdPrime;
            player.VolatilityNew = sigmaPrime;
            player.OpponentAvgRating = preAvgRatings[player.DiscordUserId];
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

    private double SolveVolatility(double sigma, double phi, double delta, double v)
    {
        double a = Math.Log(sigma * sigma);
        double A = a;
        double B;

        if (delta * delta > ((phi * phi) + v))
        {
            B = Math.Log((delta * delta) - (phi * phi) - v);
        }
        else
        {
            int k = 1;
            B = a - (k * Tau);
            while (F(B, a, delta, phi, v) > 0.0)
            {
                k++;
                B = a - (k * Tau);
                if (k > 100) break;
            }
        }

        double fA = F(A, a, delta, phi, v);
        double fB = F(B, a, delta, phi, v);

        int iters = 0;
        while (Math.Abs(B - A) > VolatilityTolerance && iters < MaxVolatilityIterations)
        {
            iters++;

            double C = A + ((A - B) * fA / (fB - fA));
            double fC = F(C, a, delta, phi, v);

            if (fC * fB < 0)
            {
                A = B;
                fA = fB;
            }
            else
            {
                fA /= 2.0;
            }

            B = C;
            fB = fC;
        }

        double x = A;
        double sigmaPrime = Math.Exp(x / 2.0);
        return sigmaPrime;
    }

    private double F(double x, double a, double delta, double phi, double v)
    {
        double ex = Math.Exp(x);
        double num = ex * ((delta * delta) - (phi * phi) - v - ex);
        double denom = 2.0 * Math.Pow((phi * phi) + v + ex, 2.0);
        double term1 = num / denom;
        double term2 = (x - a) / (Tau * Tau);
        return term1 - term2;
    }

    private static double G(double phi)
    {
        double denom = Math.Sqrt(1.0 + (3.0 * phi * phi / (Math.PI * Math.PI)));
        return 1.0 / denom;
    }

    // Numerically stable logistic
    private static double EFunc(double mu, double mu_j, double phi_j)
    {
        double g = G(phi_j);
        double d = g * (mu - mu_j);
        if (d >= 0)
        {
            double z = Math.Exp(-d);
            return 1.0 / (1.0 + z);
        }
        else
        {
            double z = Math.Exp(d);
            return z / (1.0 + z);
        }
    }
}
