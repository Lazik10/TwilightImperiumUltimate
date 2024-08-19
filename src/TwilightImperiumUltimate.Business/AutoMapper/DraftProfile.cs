using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class DraftProfile : Profile
{
    public DraftProfile()
    {
        CreateMap<FactionDraftResult, FactionDraftResultDto>()
            .ConstructUsing(f => new FactionDraftResultDto(f.PlayerFactions));

        CreateMap<FactionColorDraftResult, FactionColorDraftResultDto>();
    }
}
