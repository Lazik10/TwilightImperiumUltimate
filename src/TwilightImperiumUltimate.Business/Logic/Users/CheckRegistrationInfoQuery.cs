using TwilightImperiumUltimate.Contracts.ApiContracts.User;

namespace TwilightImperiumUltimate.Business.Logic.Users;

public class CheckRegistrationInfoQuery(
    string email,
    string username)
    : IRequest<UserRegistrationPrecheckResponse>
{
    public string Email { get; set; } = email;

    public string Username { get; set; } = username;
}
