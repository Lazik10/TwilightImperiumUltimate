namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserCommand(
    TwilightImperiumUserDto userDto) : IRequest<TwilightImperiumUserDto>
{
    public TwilightImperiumUserDto User { get; set; } = userDto;
}
