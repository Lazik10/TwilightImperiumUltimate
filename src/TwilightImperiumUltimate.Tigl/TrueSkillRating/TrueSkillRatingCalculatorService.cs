using Moserware.Skills;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Tigl.Extensions;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public class TrueSkillRatingCalculatorService : ITrueSkillRatingCalculatorService
{
    private const double InitialMu = 25.0;
    private const double InitialSigma = InitialMu / 3.0;
    private const double Beta = 25.0 / 6.0;
    private const double DynamicFactor = 0.083333;

    private readonly GameInfo _gameInfo = new GameInfo(InitialMu, InitialSigma , Beta, DynamicFactor, 0.1);

    public Task UpdatePlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, int season)
    {
        ArgumentNullException.ThrowIfNull(matchStats);

        // FFA rule: each player is a separate 1-player team.
        // Order by placement so 'teams' aligns with 'ranks'.
        var ordered = matchStats
            .OrderBy(p => p.Placement)
            .ThenBy(p => p.DiscordUserId) // deterministic within same placement
            .ToList();

        var teams = new List<IDictionary<Player, Rating>>(ordered.Count);
        var ranks = new List<int>(ordered.Count);
        var playerMap = new Dictionary<long, Player>(ordered.Count);

        foreach (var ps in ordered)
        {
            var player = new Player(ps.DiscordUserId);
            playerMap[ps.DiscordUserId] = player;

            // 1-player "team"
            teams.Add(new Dictionary<Player, Rating>
            {
                [player] = new Rating(ps.MuOld, ps.SigmaOld),
            });

            // Rank is the placement; ties are represented by equal numbers here
            ranks.Add(ps.Placement);
        }

        // Calculate new ratings (returns IDictionary<Player, Rating>)
        var newRatings = TrueSkillCalculator.CalculateNewRatings(_gameInfo, teams, ranks.ToArray());

        // Write back to the original collection
        foreach (var ps in matchStats)
        {
            var player = playerMap[ps.DiscordUserId];
            var rating = newRatings[player];

            ps.MuNew = rating.Mean;
            ps.SigmaNew = rating.StandardDeviation;
            ps.OpponentAvgRating = CalculateAverageRating(matchStats, ps.DiscordUserId);
            ps.Season = season;
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

    private static double CalculateAverageRating(IEnumerable<TrueSkillPlayerMatchStats> opponents, long playerDiscordId)
    {
        return opponents.Where(x => x.DiscordUserId != playerDiscordId).Average(x => x.MuOld);
    }
}
