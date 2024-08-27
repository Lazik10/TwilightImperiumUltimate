using TwilightImperiumUltimate.Contracts.DTOs.Unit;

namespace TwilightImperiumUltimate.Web.AutoMapper;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<UnitWithCountDto, UnitModel>();
    }
}
