using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetRecentAchievementsQueryHandler(
    IAchievementRepository achievementRepository)
    : IRequestHandler<GetRecentAchievementsQuery, ItemListDto<RankingsAchievementDto>>
{
    private readonly IAchievementRepository _repo = achievementRepository;

    public async Task<ItemListDto<RankingsAchievementDto>> Handle(GetRecentAchievementsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rows = await _repo.GetRecentAchievements(request.Take, cancellationToken);
        var totalUsers = await _repo.GetTotalTiglUsers(cancellationToken);
        var achievementPlayerCounts = await _repo.GetAchievementPlayerCounts(cancellationToken);

        var dtos = rows.Select(a => new RankingsAchievementDto
        {
            TiglUserId = a.TiglUserId,
            TiglUserName = a.TiglUser.TiglUserName,
            AchievementName = a.AchievementName,
            Category = a.Achievement.Category,
            Faction = a.Faction,
            AchievedAt = a.AchievedAt,
            MatchId = a.MatchId,
            MatchName = a.MatchName,
            RarityPercent = GetRarityPercent(a.AchievementName, totalUsers, achievementPlayerCounts),
        }).ToList();

        return new ItemListDto<RankingsAchievementDto>(dtos);
    }

    private static double GetRarityPercent(AchievementName achievementName, int totalUsers, IReadOnlyDictionary<AchievementName, int> achievementPlayerCounts)
    {
        if (totalUsers <= 0)
            return 0;

        return achievementPlayerCounts.TryGetValue(achievementName, out var count)
            ? (double)count / totalUsers * 100
            : 0;
    }
}
