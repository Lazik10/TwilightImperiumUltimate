using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public record GetAllMatchReportsQuery : IRequest<List<MatchReportDto>>;