using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with 5 more points than any other player.
/// </summary>
[AchievementEvaluator(AchievementName.CrusherOfEnemies)]
public sealed class CrusherOfEnemiesAchievementEvaluator(
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        // Get winners who don't have this achievement yet
        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        // Award achievement to winners who outscored all other players by at least 5 points
        foreach (var winner in winners.Where(winner => matchReport.PlayerResults
            .Where(x => x.TiglUserId != winner.TiglUserId)
            .All(x => winner.Score >= x.Score + 5)))
        {
            await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
