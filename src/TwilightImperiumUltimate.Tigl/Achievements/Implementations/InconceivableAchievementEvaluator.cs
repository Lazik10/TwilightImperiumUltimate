using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win 5 games in a row.
/// </summary>
[AchievementEvaluator(AchievementName.Inconceivable)]
public sealed class InconceivableAchievementEvaluator(
    IAchievementRepository achievementRepository,
    ITiglUserRepository tiglUserRepository)
    : IAchievementEvaluator
{
    // Only need to check last 4 games, as the 5th is the current win
    private const int LastFourGameResults = 4;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        var winners = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

        foreach (var winner in winners)
        {
            var factionResults = await tiglUserRepository.GetTiglUserMatchReports(winner.TiglUserId, matchReport.EndTimestamp, cancellationToken, false);
            var lastFiveResults = factionResults
                .OrderByDescending(gr => gr.EndTimestamp)
                .Take(LastFourGameResults)
                .ToList();

            if (lastFiveResults is null || lastFiveResults.Count < LastFourGameResults)
                continue;

            var lastFiveGamesAreWins = lastFiveResults
                .SelectMany(mr => mr.PlayerResults)
                .Where(pr => pr.TiglUserId == winner.TiglUserId)
                .Take(LastFourGameResults)
                .All(pr => pr.IsWinner);

            if (lastFiveGamesAreWins)
                await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken);
        }
    }
}
