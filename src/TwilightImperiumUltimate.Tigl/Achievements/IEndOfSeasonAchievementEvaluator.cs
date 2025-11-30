using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Tigl.Achievements;

public interface IEndOfSeasonAchievementEvaluator
{
    Task EvaluateAsync(Season season, AchievementName achievementName, CancellationToken cancellationToken = default);
}
