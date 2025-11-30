using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win in the longest game duration of the season.
/// </summary>
[AchievementEndOfSeasonEvaluator(AchievementName.Marathoner)]
public sealed class MarathonerAchievementEvaluator(
    ISeasonRepository seasonRepository,
    IAchievementRepository achievementRepository)
    : IEndOfSeasonAchievementEvaluator
{
    public async Task EvaluateAsync(Season season, AchievementName achievementName, CancellationToken cancellationToken = default)
    {
        var slowestGameThundersEdge = await seasonRepository.GetSlowestGameInSeason(season.SeasonNumber, TiglLeague.ThundersEdge, cancellationToken);
        var slowestGameFractured = await seasonRepository.GetSlowestGameInSeason(season.SeasonNumber, TiglLeague.Fractured, cancellationToken);

        if (slowestGameThundersEdge is null || slowestGameFractured is null)
            return;

        var games = new List<MatchReport>() { slowestGameFractured, slowestGameThundersEdge };

        foreach (var slowestGame in games)
        {
            if (slowestGame is not null)
            {
                foreach (var player in slowestGame.PlayerResults.Where(x => x.IsWinner))
                {
                    await achievementRepository.AwardAchievement(
                        player.TiglUserId,
                        slowestGame,
                        achievementName,
                        cancellationToken);
                }
            }
        }
    }
}
