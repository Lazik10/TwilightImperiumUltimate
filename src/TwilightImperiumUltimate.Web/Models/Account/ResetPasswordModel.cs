namespace TwilightImperiumUltimate.Web.Models.Account;

public class ResetPasswordModel
{
    public string Email { get; set; } = string.Empty;

    public string? ResetCode { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmNewPassword { get; set; } = string.Empty;
}
