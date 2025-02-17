namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGameFunNamesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetAllAsyncGameFunNamesQuery, IReadOnlyCollection<string>>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<IReadOnlyCollection<string>> Handle(GetAllAsyncGameFunNamesQuery request, CancellationToken cancellationToken)
    {
        var gameNames = await _asyncStatsRepository.GetAllAsyncFunGameNames(cancellationToken);
        return gameNames;
    }
}
