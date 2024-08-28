namespace TwilightImperiumUltimate.Business.Logic.Users;

public class AddUserToRoleCommand(
    string userId,
    string roleName) : IRequest<bool>
{
    public string UserId { get; set; } = userId;

    public string RoleName { get; set; } = roleName;
}
