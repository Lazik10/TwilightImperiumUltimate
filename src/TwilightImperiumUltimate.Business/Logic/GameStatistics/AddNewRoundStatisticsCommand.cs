using TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

namespace TwilightImperiumUltimate.Business.Logic.GameStatistics;

public class AddNewRoundStatisticsCommand(RoundStatisticsDto roundStatistics) : IRequest<bool>
{
    public RoundStatisticsDto RoundStatistics { get; } = roundStatistics;
}
