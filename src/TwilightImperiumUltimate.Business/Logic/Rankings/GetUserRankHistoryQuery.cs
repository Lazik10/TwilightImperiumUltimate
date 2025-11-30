using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetUserRankHistoryQuery(int TiglUserId) : IRequest<ItemListDto<RankHistoryDto>>;
