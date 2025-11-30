using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetAllMatchReportsQueryHandler(
    ITiglRepository tiglRepository)
    : IRequestHandler<GetAllMatchReportsQuery, List<MatchReportDto>>
{
    public async Task<List<MatchReportDto>> Handle(GetAllMatchReportsQuery request, CancellationToken cancellationToken)
    {
        var matchReports = await tiglRepository.GetAllMatchReports(cancellationToken);

        var matchReportDtos = matchReports.Select(mr => new MatchReportDto
        {
            Id = mr.Id,
            GameId = mr.GameId,
            Source = mr.Source,
            StartTimestamp = mr.StartTimestamp,
            EndTimestamp = mr.EndTimestamp,
            State = mr.State,
            League = mr.League,
            Season = mr.Season,
            PlayerResults = mr.PlayerResults?.Select(pr => new PlayerResultDto
            {
                TiglUserId = pr.TiglUserId,
                Score = pr.Score,
                Faction = pr.Faction,
            }).ToList() ?? new List<PlayerResultDto>(),
        }).ToList();

        return matchReportDtos;
    }
}