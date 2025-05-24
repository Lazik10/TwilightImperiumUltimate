namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGameFunNamesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetAllAsyncGameFunNamesQuery, IReadOnlyCollection<string>>
{
    private const string FogOfWar = "fow";
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<IReadOnlyCollection<string>> Handle(GetAllAsyncGameFunNamesQuery request, CancellationToken cancellationToken)
    {
        var gameNames = await _asyncStatsRepository.GetAllAsyncFunGameNames(cancellationToken);
        var gameNamesWithoutFow = gameNames.Where(x => !x.StartsWith(FogOfWar, StringComparison.InvariantCultureIgnoreCase)).ToList();
        return gameNamesWithoutFow;
    }
}
