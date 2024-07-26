namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class UserByEmailRequest
{
    required public string Email { get; set; } = string.Empty;
}
