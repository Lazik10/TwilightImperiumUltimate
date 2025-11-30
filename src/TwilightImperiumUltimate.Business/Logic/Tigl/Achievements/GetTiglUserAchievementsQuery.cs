using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Achievements;

public record GetTiglUserAchievementsQuery(int TiglUserId) : IRequest<ItemListDto<TiglUserAchievementDto>>;
