namespace TwilightImperiumUltimate.Web.Models.Account;

public class LoginResponse
{
    public string? TokenType { get; set; }

    public string? AccessToken { get; set; }

    public int? ExpiresIn { get; set; }

    public string? RefreshToken { get; set; }
}
