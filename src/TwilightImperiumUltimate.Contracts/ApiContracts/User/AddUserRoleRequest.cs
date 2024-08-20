namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class AddUserRoleRequest
{
    public required string UserId { get; set; }

    public required string RoleName { get; set; }
}
