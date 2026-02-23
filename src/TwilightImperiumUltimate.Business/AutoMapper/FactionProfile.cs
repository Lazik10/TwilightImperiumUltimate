namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

public class FactionProfile : Profile
{
    public FactionProfile()
    {
        CreateMap<Faction, FactionDto>()
            .ForMember(d => d.StartingTechnologies, opt => opt.Ignore())
            .ConstructUsing((f, context) =>
            {
                var units = f.FactionUnits.Aggregate(new List<UnitWithCountDto>(), (list, fu) =>
                {
                    var unitDto = context.Mapper.Map<UnitDto>(fu.Unit);
                    list.Add(new UnitWithCountDto(
                        unitDto.Id,
                        unitDto.UnitName,
                        unitDto.UnitType,
                        unitDto.Cost,
                        unitDto.Combat,
                        unitDto.Move,
                        unitDto.Capacity,
                        fu.Count));
                    return list;
                });

                var technologies = f.FactionTechnologies.Aggregate(new List<TechnologyDto>(), (list, ft) =>
                {
                    var technologyDto = context.Mapper.Map<TechnologyDto>(ft.Technology);
                    list.Add(new TechnologyDto(
                        technologyDto.Id,
                        technologyDto.TechnologyName,
                        technologyDto.Type,
                        technologyDto.Level,
                        technologyDto.IsFactionTechnology,
                        technologyDto.FactionName,
                        technologyDto.Name,
                        technologyDto.Text,
                        technologyDto.GameVersion));
                    return list;
                });

                return new FactionDto(
                f.Id,
                f.FactionName,
                f.HomeSystem,
                f.Commodities,
                f.ComplexityRating,
                f.Action,
                f.History,
                f.Quote,
                f.SystemStats,
                f.SystemInfo,
                f.GameVersion,
                units,
                technologies,
                new List<PromissoryNoteCardName>(),
                new List<BreakthroughName>());
            });
    }
}
