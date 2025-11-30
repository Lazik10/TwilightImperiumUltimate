using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Business.Logic.Rankings;

public class GetTiglLeadersOverviewQueryHandler(IRankingsRepository rankingsRepository)
    : IRequestHandler<GetTiglLeadersOverviewQuery, ItemListDto<RankingsLeaderDto>>
{
    private readonly IRankingsRepository _repo = rankingsRepository;

    public async Task<ItemListDto<RankingsLeaderDto>> Handle(GetTiglLeadersOverviewQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rows = await _repo.GetLeadersOverview(cancellationToken);
        return new ItemListDto<RankingsLeaderDto>(rows);
    }
}
