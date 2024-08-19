namespace TwilightImperiumUltimate.Web.Models.Account;

public class ConfirmEmailRequest
{
    public string UserId { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}
