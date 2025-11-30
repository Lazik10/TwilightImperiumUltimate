using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class GameReportRequest
{
    public IGameReport GameReport { get; set; } = new GameReport();
}
