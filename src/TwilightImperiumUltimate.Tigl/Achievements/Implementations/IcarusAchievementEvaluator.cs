using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Lose a game after a winning streak of 4, 9, or 14 games.
/// </summary>
[AchievementEvaluator(AchievementName.Icarus)]
public sealed class IcarusAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    private const int MaxStreakToCheck = 10;
    private static readonly HashSet<int> RequiredStreaks = [4, 9];

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var nonWinners = matchReport.PlayerResults
            .Where(pr => !pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var player in nonWinners)
        {
            var previousResults = await tiglUserRepository.GetTiglUserMatchReports(player.TiglUserId, matchReport.EndTimestamp, cancellationToken, false, null, MaxStreakToCheck);
            var orderedResults = previousResults
                .OrderByDescending(gr => gr.EndTimestamp)
                .ToList();

            var winningStreak = 0;
            foreach (var gameResult in orderedResults)
            {
                var playerResult = gameResult.PlayerResults.SingleOrDefault(pr => pr.TiglUserId == player.TiglUserId);
                if (playerResult is null || !playerResult.IsWinner)
                    break;

                winningStreak++;
            }

            if (RequiredStreaks.Contains(winningStreak))
                await achievementRepository.AwardAchievement(player.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
