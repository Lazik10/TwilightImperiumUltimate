using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Tigl.Achievements;

public interface IAchievementService
{
    Task AddGame(int gameId, CancellationToken cancellationToken = default);

    Task EvaluatePendingAsync(CancellationToken cancellationToken = default);

    Task EvaluateEndOfSeasonAchievements(Season season, CancellationToken cancellationToken = default);
}
