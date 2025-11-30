using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win against 3+ players with higher ranks than you.
/// </summary>
[AchievementEvaluator(AchievementName.UnderdogStory)]
public sealed class UnderdogStoryAchievementEvaluator(
    ITiglUserRepository tiglUserRepository,
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    private const int RequiredHigherRankedOpponents = 3;

    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        var playerRanks = new Dictionary<int, TiglRankName>();

        var users = await tiglUserRepository.GetUsersByIds(matchReport.PlayerResults.Select(pr => pr.TiglUserId!).ToHashSet(), cancellationToken);

        foreach (var user in users)
        {
            var userRanks = user.TiglRanks;

            var userRank = userRanks!
                .Where(x => x.AchievedAt < matchReport.StartTimestamp && x.League == matchReport.League)
                .OrderByDescending(x => x.AchievedAt)
                .FirstOrDefault();

            playerRanks[user.Id] = userRank!.Name;
        }

        foreach (var winner in matchReport.PlayerResults.Where(pr => pr.IsWinner))
        {
            var winnerRank = playerRanks[winner.TiglUserId!];
            var higherRankedOpponentsCount = matchReport.PlayerResults.Count(pr => playerRanks[pr.TiglUserId] > winnerRank);

            if (higherRankedOpponentsCount >= RequiredHigherRankedOpponents)
            {
                await achievementRepository.AwardAchievement(
                    winner.TiglUserId,
                    matchReport,
                    achievementName,
                    cancellationToken);
            }
        }
    }
}
