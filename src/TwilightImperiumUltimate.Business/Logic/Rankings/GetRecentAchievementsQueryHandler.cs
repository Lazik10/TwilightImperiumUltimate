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
        }).ToList();

        return new ItemListDto<RankingsAchievementDto>(dtos);
    }
}