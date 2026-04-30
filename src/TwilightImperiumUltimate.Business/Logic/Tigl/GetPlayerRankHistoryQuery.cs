using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetPlayerRankHistoryQuery(IReadOnlyCollection<long> DiscordUserIds) : IRequest<ItemListDto<TiglPlayerRankHistoryDto>>;
