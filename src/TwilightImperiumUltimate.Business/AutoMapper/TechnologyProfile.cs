namespace TwilightImperiumUltimate.Business.AutoMapper;

public class TechnologyProfile : Profile
{
    public TechnologyProfile()
    {
        CreateMap<Technology, TechnologyDto>()
            .ConstructUsing(x => new TechnologyDto(
                x.Id,
                x.TechnologyName,
                x.Type,
                x.Level,
                x.IsFactionTechnology,
                x.FactionName,
                x.Name,
                x.Text,
                x.GameVersion));
    }
}
