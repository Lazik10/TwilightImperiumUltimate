namespace TwilightImperiumUltimate.Business.Logic.Async;

public class GetAllAsyncGameNamesQueryHandler(
    IAsyncStatsRepository asyncStatsRepository)
    : IRequestHandler<GetAllAsyncGameNamesQuery, IReadOnlyCollection<string>>
{
    private readonly IAsyncStatsRepository _asyncStatsRepository = asyncStatsRepository;

    public async Task<IReadOnlyCollection<string>> Handle(GetAllAsyncGameNamesQuery request, CancellationToken cancellationToken)
    {
        var gameNames = await _asyncStatsRepository.GetAllAsyncGameNames(cancellationToken);
        return gameNames;
    }
}
