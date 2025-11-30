using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetAllSeasonsQuery() : IRequest<ItemListDto<SeasonDto>>;
