using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public record GetTiglLeadersOverviewQuery() : IRequest<ItemListDto<RankingsLeaderDto>>;
