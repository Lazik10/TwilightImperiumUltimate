using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public class GetTiglUserAchievementsQueryHandler(IAchievementRepository achievementRepository)
    : IRequestHandler<GetTiglUserAchievementsQuery, ItemListDto<TiglUserAchievementDto>>
{
    public async Task<ItemListDto<TiglUserAchievementDto>> Handle(GetTiglUserAchievementsQuery request, CancellationToken cancellationToken)
    {
        var items = await achievementRepository.GetUserAchievements(request.TiglUserId, cancellationToken);
        var dtoItems = items.Select(a => new TiglUserAchievementDto
        {
            TiglUserId = a.TiglUserId,
            AchievementName = a.AchievementName,
            Category = a.Achievement.Category,
            Faction = a.Faction,
            AchievedAt = a.AchievedAt,
            MatchId = a.MatchId,
            MatchName = a.MatchName,
        }).ToList();

        return new ItemListDto<TiglUserAchievementDto>(dtoItems);
    }
}
