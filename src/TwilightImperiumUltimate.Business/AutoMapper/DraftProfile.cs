using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class DraftProfile : Profile
{
    public DraftProfile()
    {
        CreateMap<FactionDraftResult, FactionDraftResultDto>();
        CreateMap<FactionColorDraftResult, FactionColorDraftResultDto>();
    }
}
