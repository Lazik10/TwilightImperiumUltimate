using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGamesByYearAndMonthQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllAsyncGamesByYearAndMonthQuery, List<AsyncGameDto>>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<AsyncGameDto>> Handle(GetAllAsyncGamesByYearAndMonthQuery request, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGamesByYearAndMonthQuery(request.Year, request.Month, cancellationToken);
        var gamesDto = _mapper.Map<List<AsyncGameDto>>(games);

        return gamesDto;
    }
}
