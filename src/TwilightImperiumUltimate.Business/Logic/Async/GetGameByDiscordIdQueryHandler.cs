using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetGameByDiscordIdQueryHandler(
    IAsyncStatsRepository asyncStatsRepository,
    IMapper mapper)
    : IRequestHandler<GetGameByDiscordIdQuery, AsyncGameDto>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<AsyncGameDto> Handle(GetGameByDiscordIdQuery request, CancellationToken cancellationToken)
    {
        var game = await _asyncStatsRepository.GetAsyncGameByDiscordId(request.GameDiscordId, cancellationToken);

        if (game is not null)
            return _mapper.Map<AsyncGameDto>(game);

        return new AsyncGameDto(-1, string.Empty, string.Empty, 0, 0, false, false, 0, 0, 0, false);
    }
}
