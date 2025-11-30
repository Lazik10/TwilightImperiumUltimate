using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetRecentAchievementsQuery(int Take = 50) : IRequest<ItemListDto<RankingsAchievementDto>>;