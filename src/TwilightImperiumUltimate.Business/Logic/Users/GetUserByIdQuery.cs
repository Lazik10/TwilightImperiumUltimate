namespace TwilightImperiumUltimate.Business.Logic.Users;

public class GetUserByIdQuery(
    string id)
    : IRequest<TwilightImperiumUserDto>
{
    public string Id { get; set; } = id;
}
