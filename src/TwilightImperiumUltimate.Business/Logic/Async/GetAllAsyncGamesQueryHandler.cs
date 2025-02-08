using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGamesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllAsyncGamesQuery, List<AsyncGameDto>>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<AsyncGameDto>> Handle(GetAllAsyncGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);
        var gamesDto = _mapper.Map<List<AsyncGameDto>>(games);

        return gamesDto;
    }
}
