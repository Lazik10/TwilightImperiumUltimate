using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IAchievementRepository
{
    Task AwardFactionAchievement(int tiglUserId, MatchReport matchReport, AchievementName achievementName, TiglFactionName faction, int minWins, CancellationToken cancellationToken);

    Task AwardAchievement(int tiglUserId, MatchReport matchReport, AchievementName achievementName, CancellationToken cancellationToken, TiglFactionName faction = TiglFactionName.None);

    Task<List<TiglUserAchievement>> GetUserAchievements(int tiglUserId, CancellationToken cancellationToken);

    Task<List<TiglUserAchievement>> GetUsersAchievements(IEnumerable<int> tiglUserIds, CancellationToken cancellationToken);

    Task<List<TiglUserAchievement>> GetRecentAchievements(int take, CancellationToken cancellationToken);

    Task<bool> AddManualAchievement(int tiglUserId, AchievementName achievementName, TiglFactionName faction, CancellationToken cancellationToken);

    Task<bool> RemoveAchievement(int tiglUserId, AchievementName achievementName, TiglFactionName faction, CancellationToken cancellationToken);
}
