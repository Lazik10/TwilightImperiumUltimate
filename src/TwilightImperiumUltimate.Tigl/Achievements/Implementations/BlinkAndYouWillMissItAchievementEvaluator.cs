using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game in round 3 or earlier.
/// </summary>
[AchievementEvaluator(AchievementName.BlinkAndYouWillMissIt)]
public sealed class BlinkAndYouWillMissItAchievementEvaluator(
    IAchievementRepository achievementRepository) : IAchievementEvaluator
{
    private const int MaxRoundForAchievement = 3;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var legalLeague = matchReport.League is TiglLeague.ThundersEdge or TiglLeague.Fractured;
        if (!legalLeague)
            return;

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        if (matchReport.Round <= MaxRoundForAchievement)
        {
            await Task.WhenAll(winners.Select(winner =>
                achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken)));
        }
    }
}
