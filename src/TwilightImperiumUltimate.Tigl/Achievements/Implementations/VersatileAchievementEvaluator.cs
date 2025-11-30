using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win at least one game in each VP amount (10, 12, 14).
/// </summary>
[AchievementEvaluator(AchievementName.Versatile)]
public sealed class VersatileAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private readonly HashSet<int> _requiredVPWins = [10, 12, 14];

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(x => x.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == x.TiglUserId && ua.AchievementName == achievementName))
            .Select(x => x.TiglUserId);

        foreach (var winner in winners)
        {
            var games = await tiglUserRepository.GetTiglUserMatchReports(winner, matchReport.EndTimestamp, cancellationToken, true);
            games.Add(matchReport);

            var victoryPointsWins = games.Where(g => g.PlayerResults.Any(pr => pr.TiglUserId == winner && pr.IsWinner))
                .Select(g => g.Score)
                .Distinct()
                .ToHashSet();

            if (victoryPointsWins.SetEquals(_requiredVPWins))
                await achievementRepository.AwardAchievement(winner, matchReport, achievementName, cancellationToken);
        }
    }
}
