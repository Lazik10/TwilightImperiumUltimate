using TwilightImperiumUltimate.Contracts.Enums;
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
        ArgumentNullException.ThrowIfNull(season);

        var slowestGameThundersEdge = await seasonRepository.GetSlowestGameInSeason(season.SeasonNumber, TiglLeague.ThundersEdge, cancellationToken);
        var slowestGameFractured = await seasonRepository.GetSlowestGameInSeason(season.SeasonNumber, TiglLeague.Fractured, cancellationToken);

        if (slowestGameThundersEdge is null || slowestGameFractured is null)
            return;

        var games = new List<MatchReport>() { slowestGameFractured, slowestGameThundersEdge };

        foreach (var (slowestGame, player) in games.Where(slowestGame => slowestGame is not null)
            .SelectMany(slowestGame => slowestGame.PlayerResults.Where(x => x.IsWinner).Select(player => (slowestGame, player))))
        {
            await achievementRepository.AwardAchievement(player.TiglUserId, slowestGame, achievementName, cancellationToken);
        }
    }
}
