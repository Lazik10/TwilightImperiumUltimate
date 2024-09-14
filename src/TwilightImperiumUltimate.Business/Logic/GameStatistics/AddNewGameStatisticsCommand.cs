using TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

namespace TwilightImperiumUltimate.Business.Logic.GameStatistics;

public class AddNewGameStatisticsCommand(
    GameStatisticsDto gameStatisticsDto)
    : IRequest<bool>
{
    public GameStatisticsDto GameStatisticsDto { get; } = gameStatisticsDto;
}
