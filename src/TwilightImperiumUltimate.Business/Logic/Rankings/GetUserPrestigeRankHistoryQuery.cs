using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetUserPrestigeRankHistoryQuery(int TiglUserId) : IRequest<ItemListDto<PrestigeRankHistoryDto>>;
