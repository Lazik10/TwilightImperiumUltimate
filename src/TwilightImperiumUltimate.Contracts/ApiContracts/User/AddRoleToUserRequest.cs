namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class AddRoleToUserRequest
{
    public required string UserId { get; set; }

    public required string RoleName { get; set; }
}