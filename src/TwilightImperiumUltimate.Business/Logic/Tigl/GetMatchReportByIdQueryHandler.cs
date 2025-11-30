using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetMatchReportByIdQueryHandler(
    ITiglRepository tiglRepository,
    ITiglUserRepository tiglUserRepository,
    IMapper mapper)
    : IRequestHandler<GetMatchReportByIdQuery, MatchReportDto?>
{
    public async Task<MatchReportDto?> Handle(GetMatchReportByIdQuery request, CancellationToken cancellationToken)
    {
        var matchReport = await tiglRepository.GetMatchReportWithPlayerResults(request.Id, cancellationToken);
        var tiglUsers = await tiglUserRepository.GetTiglUsersBaseInfoById(
            matchReport?.PlayerResults?.Select(pr => pr.TiglUserId).ToList() ?? new List<int>(),
            cancellationToken);

        if (matchReport == null)
            return null;

        var matchReportDto = mapper.Map<MatchReportDto>(matchReport);

        foreach (var playerResultDto in matchReportDto.PlayerResults)
        {
            var tiglUser = tiglUsers.FirstOrDefault(u => u.Id == playerResultDto.TiglUserId);
            if (tiglUser != null)
            {
                playerResultDto.DiscordUserName = tiglUser.DiscordTag;
                playerResultDto.TiglUserName = tiglUser.TiglUserName;

                var asyncStats = matchReportDto.PlayerMatchAsyncStats.FirstOrDefault(x => x.DiscordUserId == tiglUser.DiscordId);
                if (asyncStats is not null)
                    asyncStats.TiglUserId = tiglUser.Id;

                var glickoStats = matchReportDto.PlayerMatchGlickoStats.FirstOrDefault(x => x.DiscordUserId == tiglUser.DiscordId);
                if (glickoStats is not null)
                    glickoStats.TiglUserId = tiglUser.Id;

                var trueSkillStats = matchReportDto.PlayerMatchTrueSkillStats.FirstOrDefault(x => x.DiscordUserId == tiglUser.DiscordId);
                if (trueSkillStats is not null)
                    trueSkillStats.TiglUserId = tiglUser.Id;
            }
        }

        matchReportDto.GalacticEvents = TiglGalacticEventConverter.ConvertFromFlag(matchReport.Events);

        return matchReportDto;
    }
}