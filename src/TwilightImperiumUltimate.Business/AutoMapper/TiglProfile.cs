using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Business.AutoMapper;

internal class TiglProfile : Profile
{
    public TiglProfile()
    {
        CreateMap<PlayerResult, PlayerResultDto>();
        CreateMap<MatchReport, MatchReportDto>();
        CreateMap<AsyncPlayerMatchStats, AsyncPlayerMatchStatsDto>();
        CreateMap<GlickoPlayerMatchStats, GlickoPlayerMatchStatsDto>();
        CreateMap<TrueSkillPlayerMatchStats, TrueSkillPlayerMatchStatsDto>();
    }
}
