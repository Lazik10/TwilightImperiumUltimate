using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class ReportGameCommand(GameReport gameReport) : IRequest<GameReportResult>
{
    public GameReport GameReport { get; set; } = gameReport;
}
