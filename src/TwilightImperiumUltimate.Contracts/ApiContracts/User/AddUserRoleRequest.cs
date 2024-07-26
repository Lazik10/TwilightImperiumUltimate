namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class AddUserRoleRequest
{
    required public string UserId { get; set; }

    required public string RoleName { get; set; }
}
