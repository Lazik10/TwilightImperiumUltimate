using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win 15 games as [Faction].
/// </summary>
[AchievementEvaluator(AchievementName.FactionExpert)]
public sealed class FactionExpertAchievementEvaluator(
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    private const int MinWinsWithFaction = 15;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner
                && !usersAchievements.Any(ua =>
                    ua.TiglUserId == pr.TiglUserId
                    && ua.Achievement.Name == achievementName
                    && ua.Faction == pr.Faction))
            .ToList();

        foreach (var winner in winners)
        {
            await achievementRepository.AwardFactionAchievement(
                winner.TiglUserId,
                matchReport,
                achievementName,
                winner.Faction,
                MinWinsWithFaction,
                cancellationToken);
        }
    }
}
