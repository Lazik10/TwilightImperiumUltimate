using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record EvaluateGameReportCommand(int MatchReportId) : IRequest<GameReportResult>;
