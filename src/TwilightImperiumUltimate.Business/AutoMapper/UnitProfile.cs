using TiUnit = TwilightImperiumUltimate.Core.Entities.Units.Unit;

namespace TwilightImperiumUltimate.Business.AutoMapper;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<TiUnit, UnitDto>()
            .ConstructUsing(x => new UnitDto(
                x.Id,
                x.UnitName,
                x.UnitType,
                x.Cost,
                x.Combat,
                x.Move,
                x.Capacity
            ));
    }
}
