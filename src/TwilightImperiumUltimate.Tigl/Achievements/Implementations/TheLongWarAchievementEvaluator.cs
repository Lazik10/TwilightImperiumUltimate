using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game to 14 VPs.
/// </summary>
[AchievementEvaluator(AchievementName.TheLongWar)]
public sealed class TheLongWarAchievementEvaluator(
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    private const int VictoryPointsToWin = 14;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(x => x.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == x.TiglUserId && ua.AchievementName == achievementName))
            .Select(x => x.TiglUserId);

        foreach (var player in winners)
        {
            if (matchReport.Score == VictoryPointsToWin)
                await achievementRepository.AwardAchievement(player, matchReport, achievementName, cancellationToken);
        }
    }
}