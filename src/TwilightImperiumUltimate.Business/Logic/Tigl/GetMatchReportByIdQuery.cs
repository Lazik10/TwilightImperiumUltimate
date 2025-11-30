using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetMatchReportByIdQuery(int Id) : IRequest<MatchReportDto>;