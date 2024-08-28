namespace TwilightImperiumUltimate.Business.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<TwilightImperiumUser, TwilightImperiumUserDto>()
            .ConstructUsing(x => new TwilightImperiumUserDto(
                x.Id,
                x.UserName,
                x.FirstName,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                x.Age,
                x.FavoriteFaction,
                x.UserInfo,
                x.DiscordId,
                x.SteamId));

        CreateMap<TwilightImperiumUserDto, TwilightImperiumUser>();
    }
}
