using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;
using TwilightImperiumUltimate.Tigl.RankUp;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win a game in which you would gain a Faction hero you already have.
/// </summary>
[AchievementEvaluator(AchievementName.FlauntingIt)]
public sealed class FlauntingItAchievementEvaluator(
    IPrestigeRankService prestigeRankService,
    IAchievementRepository achievementRepository)
    : IAchievementEvaluator
{
    public async Task EvaluateAsync(MatchReport matchReport, AchievementName achievementName, IReadOnlyCollection<TiglUserAchievement> usersAchievements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(matchReport);

        // This echievement can only be scored in official leage
        if (matchReport.League == TiglLeague.ThundersEdge)
        {
            var winnersWithoutThisAchievement = matchReport.PlayerResults
            .Where(pr => pr.IsWinner && !usersAchievements.Any(ua => ua.TiglUserId == pr.TiglUserId && ua.Achievement.Name == achievementName))
            .ToList();

            foreach (var winner in winnersWithoutThisAchievement)
            {
                var hasRank = await prestigeRankService.HasFactionPrestigeRank(winner.TiglUserId, matchReport, winner.Faction, TiglLeague.ThundersEdge, cancellationToken);

                if (hasRank)
                {
                    await achievementRepository.AwardAchievement(winner.TiglUserId, matchReport, achievementName, cancellationToken, winner.Faction);
                }
            }
        }
    }
}
