namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class UserRegistrationPrecheckResponse
{
    public bool EmailNotAvailable { get; set; }

    public bool UserNameNotAvailable { get; set; }
}
