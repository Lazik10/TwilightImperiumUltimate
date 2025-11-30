using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game with all other players at 9 points.
/// </summary>
[AchievementEvaluator(AchievementName.MarrowEnjoyer)]
public sealed class MarrowEnjoyerAchievementEvaluator(
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

        // Award achievement to winners who had all other players at 9/11/13 points in 10/12/14 VP game
        foreach (var winner in winners.Where(winner => matchReport.PlayerResults
            .Where(x => x.TiglUserId != winner.TiglUserId)
            .All(pr => pr.Score >= matchReport.Score - 1)))
        {
            await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
