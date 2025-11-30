using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Tigl.Achievements;

namespace TwilightImperiumUltimate.Tigl.Services;

public class EndOfSeasonProcessor(ISeasonRepository seasonRepository,
    IAchievementService achievementService,
    IDecayService decayService,
    ISeasonLeaderboardService seasonLeaderboardService)
    : IEndOfSeasonProcessor
{
    public async Task<bool> ProcessEndOfSeason(CancellationToken cancellationToken)
    {
        var result = await seasonRepository.EndCurrentSeason(cancellationToken);
        if (result.IsSuccess)
        {
            await decayService.ApplyDecay(result.Value.SeasonNumber, cancellationToken);

            await achievementService.EvaluateEndOfSeasonAchievements(result.Value, cancellationToken);

            await seasonLeaderboardService.CreateLeaderboard(result.Value.SeasonNumber, cancellationToken);
        }

        return result.IsSuccess;
    }
}
