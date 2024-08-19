namespace TwilightImperiumUltimate.Web.Models.Account;

public class RegistrationModel
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}
