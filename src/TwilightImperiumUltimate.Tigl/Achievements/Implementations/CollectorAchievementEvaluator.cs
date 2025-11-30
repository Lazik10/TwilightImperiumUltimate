using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Earn 5 different achievements.
/// </summary>
[AchievementSummaryEvaluator(AchievementName.Collector)]
public sealed class CollectorAchievementEvaluator(
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    private const int MinAchievementsEarned = 5;

    private readonly HashSet<AchievementName> excludedAchievements =
    [
        AchievementName.Collector,
        AchievementName.Overachiever,
        AchievementName.Completionist,
    ];

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var players = matchReport.PlayerResults.Select(x => x.TiglUserId);

        foreach (var player in players)
        {
            var achievementsCount = usersAchievements
                .Where(ua => player == ua.TiglUserId && !excludedAchievements.Contains(ua.Achievement.Name))
                .Select(x => x.AchievementName)
                .Distinct()
                .Count();

            if (achievementsCount >= MinAchievementsEarned && !usersAchievements.Any(ua => ua.TiglUserId == player && ua.Achievement.Name == achievementName))
            {
                await achievementRepository.AwardAchievement(player, matchReport, achievementName, cancellationToken);
            }
        }
    }
}
