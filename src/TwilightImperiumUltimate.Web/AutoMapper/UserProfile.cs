using TwilightImperiumUltimate.Contracts.DTOs.User;
using TwilightImperiumUltimate.Web.Models.Users;

namespace TwilightImperiumUltimate.Web.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<TwilightImperiumUserDto, TwilightImperiumUser>();
        CreateMap<TwilightImperiumUser, TwilightImperiumUserDto>();
    }
}
