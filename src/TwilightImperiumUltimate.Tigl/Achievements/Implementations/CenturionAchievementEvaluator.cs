using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Finish 100 games.
/// </summary>
[AchievementEvaluator(AchievementName.Centurion)]
public sealed class CenturionAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int MinGamesPlayed = 99;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var players = matchReport.PlayerResults
            .Where(x => !usersAchievements.Any(ua => ua.TiglUserId == x.TiglUserId && ua.AchievementName == achievementName))
            .Select(x => x.TiglUserId);

        foreach (var player in players)
        {
            var playerResults = await tiglUserRepository.GetTiglUserMatchReports(player, matchReport.EndTimestamp, cancellationToken, false, null, MinGamesPlayed);

            if (playerResults is not null && playerResults.Count == MinGamesPlayed)
                await achievementRepository.AwardAchievement(player, matchReport, achievementName, cancellationToken);
        }
    }
}
