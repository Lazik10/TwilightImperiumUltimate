namespace TwilightImperiumUltimate.Business.Logic.Users;

public class RemoveRoleFromUserCommand(
    string userId,
    string roleName) : IRequest<bool>
{
    public string UserId { get; set; } = userId;

    public string RoleName { get; set; } = roleName;
}
