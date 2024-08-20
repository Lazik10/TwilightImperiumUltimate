namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class UserByEmailRequest
{
    public required string Email { get; set; } = string.Empty;
}
