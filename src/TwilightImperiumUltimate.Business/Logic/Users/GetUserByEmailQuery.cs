namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetUserByEmailQuery(string email)
    : IRequest<TwilightImperiumUserDto>
{
    public string Email { get; set; } = email;
}
