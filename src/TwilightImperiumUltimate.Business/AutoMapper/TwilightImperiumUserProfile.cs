namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

internal class TwilightImperiumUserProfile : Profile
{
    public TwilightImperiumUserProfile()
    {
        CreateMap<TwilightImperiumUser, TwilightImperiumUserDto>();
    }
}
