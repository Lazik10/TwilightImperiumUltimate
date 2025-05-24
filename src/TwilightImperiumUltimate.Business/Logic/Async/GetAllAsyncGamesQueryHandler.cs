using TwilightImperiumUltimate.Contracts.DTOs.Async;
using System.Linq;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGamesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllAsyncGamesQuery, List<AsyncGameDto>>
{
    private const string FogOfWar = "fow";
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<AsyncGameDto>> Handle(GetAllAsyncGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await _asyncStatsRepository.GetAllAsyncGames(cancellationToken);

        // Make sure active fow games stays hidden
        foreach (var game in games.Where(game => game.AsyncGameID.StartsWith(FogOfWar)))
        {
            game.AsyncGameID = FogOfWar;
        }

        var gamesDto = _mapper.Map<List<AsyncGameDto>>(games);

        return gamesDto;
    }
}
