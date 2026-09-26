using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetRecentAchievementsQuery(int Take = 100) : IRequest<ItemListDto<RankingsAchievementDto>>;
