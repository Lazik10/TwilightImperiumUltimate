using TwilightImperiumUltimate.Contracts.DTOs.Unit;
using TwilightImperiumUltimate.Web.Models.Unit;

namespace TwilightImperiumUltimate.Web.AutoMapper;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<UnitWithCountDto, UnitModel>();
    }
}
