using TwilightImperiumUltimate.Contracts.ApiContracts;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class UpdateUserNameCommand(string email, string userName) : IRequest<TwilightImperiumUserDto>
{
    public string Email { get; set; } = email;

    public string UserName { get; set; } = userName;
}
