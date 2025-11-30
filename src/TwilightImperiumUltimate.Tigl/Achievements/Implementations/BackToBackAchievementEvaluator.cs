using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win 2 games in a row as the same faction (games not as that faction can be played in between).
/// </summary>
[AchievementEvaluator(AchievementName.BackToBack)]
public sealed class BackToBackAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var winner in winners)
        {
            var faction = winner.Faction;

            var factionResults = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, false, [faction]);
            var lastFactionMatch = factionResults
                .OrderByDescending(gr => gr.EndTimestamp)
                .FirstOrDefault();

            if (lastFactionMatch is null)
                continue;

            var isWin = lastFactionMatch.PlayerResults
                .Single(pr => pr.Faction == faction && pr.TiglUserId == winner.TiglUserId).IsWinner;

            if (isWin)
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken, faction);
        }
    }
}
