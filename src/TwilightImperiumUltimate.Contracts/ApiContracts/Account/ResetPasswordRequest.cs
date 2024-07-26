namespace TwilightImperiumUltimate.Web.Models.Account;

public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;

    public string? ResetCode { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;
}
