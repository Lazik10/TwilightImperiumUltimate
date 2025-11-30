using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetFactionSeasonStatsQuery(int Season, TiglLeague League) : IRequest<ItemListDto<FactionSeasonStatsDto>>;
