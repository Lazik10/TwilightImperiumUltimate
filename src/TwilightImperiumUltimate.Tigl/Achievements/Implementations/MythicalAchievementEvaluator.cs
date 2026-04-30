using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win 10 games in a row.
/// </summary>
[AchievementEvaluator(AchievementName.Mythical)]
public sealed class MythicalAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    // Only need to check last 9 games, as the 10th is the current win
    private const int LastNineGameResults = 9;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var winner in winners)
        {
            var factionResults = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, false);
            var lastNineResults = factionResults
                .OrderByDescending(gr => gr.EndTimestamp)
                .Take(LastNineGameResults)
                .ToList();

            if (lastNineResults is null || lastNineResults.Count < LastNineGameResults)
                continue;

            var lastNineGamesAreWins = lastNineResults
                .SelectMany(mr => mr.PlayerResults)
                .Where(pr => pr.TiglUserId == winner.TiglUserId)
                .Take(LastNineGameResults)
                .All(pr => pr.IsWinner);

            if (lastNineGamesAreWins)
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}