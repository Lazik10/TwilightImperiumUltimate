using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetSeasonLeaderboardQuery(int Season) : IRequest<ItemListDto<PlayerSeasonResultDto>>;
