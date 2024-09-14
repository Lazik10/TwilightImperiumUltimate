namespace TwilightImperiumUltimate.Business.Logic.GameStatistics;

public class AddNewRoundStatisticsCommandHandler(
    IGameStatisticsRepository gameStatisticsRepository)
    : IRequestHandler<AddNewRoundStatisticsCommand, bool>
{
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;

    public async Task<bool> Handle(AddNewRoundStatisticsCommand request, CancellationToken cancellationToken)
    {
        foreach (var factionStats in request.RoundStatistics.FactionStats)
        {
            await _gameStatisticsRepository.AddNewRoundFactionStatistics(
                request.RoundStatistics.GameId,
                request.RoundStatistics.Round,
                factionStats.FactionName,
                factionStats.Score,
                factionStats.StrategyCardName,
                cancellationToken);
        }

        return true;
    }
}
