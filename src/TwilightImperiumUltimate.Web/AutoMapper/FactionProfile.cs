namespace TwilightImperiumUltimate.Web.AutoMapper;

public class FactionProfile : Profile
{
    public FactionProfile()
    {
        CreateMap<FactionDto, FactionModel>();
        CreateMap<FactionDto, MiltyDraftFactionModel>();
    }
}
