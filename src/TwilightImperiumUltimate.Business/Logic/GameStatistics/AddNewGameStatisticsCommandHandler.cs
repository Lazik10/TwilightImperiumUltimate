namespace TwilightImperiumUltimate.Business.Logic.GameStatistics;

public class AddNewGameStatisticsCommandHandler(
    IGameStatisticsRepository gameStatisticsRepository)
    : IRequestHandler<AddNewGameStatisticsCommand, bool>
{
    private readonly IGameStatisticsRepository _gameStatisticsRepository = gameStatisticsRepository;

    public async Task<bool> Handle(AddNewGameStatisticsCommand request, CancellationToken cancellationToken)
    {
        await _gameStatisticsRepository.UpdateWebsiteStatistics(StatisticsType.GamesPlayed, cancellationToken);

        await _gameStatisticsRepository.AddNewGameStatistics(
            request.GameStatisticsDto.GameId,
            request.GameStatisticsDto.MaxPoints,
            request.GameStatisticsDto.NumberOfPlayers,
            request.GameStatisticsDto.GameRound,
            request.GameStatisticsDto.Time,
            request.GameStatisticsDto.Winner,
            cancellationToken);

        return true;
    }
}
