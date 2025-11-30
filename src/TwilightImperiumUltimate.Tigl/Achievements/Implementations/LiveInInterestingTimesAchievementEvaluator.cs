using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win games that collectively included all Galactic Events.
/// </summary>
[AchievementEvaluator(AchievementName.LiveInInterestingTimes)]
public sealed class LiveInInterestingTimesAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var playersWithoutAchievement = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .Select(x => x.TiglUserId);

        foreach (var playerId in playersWithoutAchievement)
        {
            var playerMatchReports = await tiglUserRepository.GetTiglUserMatchReports(playerId, matchReport.EndTimestamp, cancellationToken, false, null);
            playerMatchReports.Add(matchReport);

            if (TiglGalacticEventConverter.AreAllGalacticEventsPresent(playerMatchReports, playerId))
            {
                await achievementRepository.AwardAchievement(playerId, matchReport, achievementName, cancellationToken);
            }
        }
    }
}
