namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class UserRegisterationPrecheckRequest
{
    public string Email { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
}
