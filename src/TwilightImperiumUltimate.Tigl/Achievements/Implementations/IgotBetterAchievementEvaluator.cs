using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game as a faction you have 5 losses with.
/// </summary>
[AchievementEvaluator(AchievementName.IGotBetter)]
public sealed class IgotBetterAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int RequiredConsecutiveLosses = 5;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var winner in winners)
        {
            var factionResults = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, false, [winner.Faction]);

            var lastFiveFactionResults = factionResults
                .OrderByDescending(gr => gr.EndTimestamp)
                .Take(RequiredConsecutiveLosses)
                .SelectMany(x => x.PlayerResults)
                .Where(x => x.TiglUserId == winner.TiglUserId)
                .ToList();

            if (lastFiveFactionResults.Count == RequiredConsecutiveLosses && lastFiveFactionResults.All(x => !x.IsWinner))
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken, winner.Faction);
        }
    }
}
