using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements.Attributes;

namespace TwilightImperiumUltimate.Tigl.Achievements.Implementations;

/// <summary>
/// Win in the shortest game duration of the season.
/// </summary>
[AchievementEndOfSeasonEvaluator(AchievementName.SpeedDemon)]
public sealed class SpeedDemonAchievementEvaluator(
    ISeasonRepository seasonRepository,
    IAchievementRepository achievementRepository)
    : IEndOfSeasonAchievementEvaluator
{
    public async Task EvaluateAsync(Season season, AchievementName achievementName, CancellationToken cancellationToken = default)
    {
        var fastestGameThundersEdge = await seasonRepository.GetFastestGameInSeason(season.SeasonNumber, TiglLeague.ThundersEdge, cancellationToken);
        var fastestGameFractured = await seasonRepository.GetFastestGameInSeason(season.SeasonNumber, TiglLeague.Fractured, cancellationToken);

        if (fastestGameThundersEdge is null || fastestGameFractured is null)
            return;

        var games = new List<MatchReport>() { fastestGameFractured, fastestGameThundersEdge };

        foreach (var fastestGame in games)
        {
            if (fastestGame is not null)
            {
                foreach (var player in fastestGame.PlayerResults.Where(x => x.IsWinner))
                {
                    await achievementRepository.AwardAchievement(
                        player.TiglUserId,
                        fastestGame,
                        achievementName,
                        cancellationToken);
                }
            }
        }
    }
}
