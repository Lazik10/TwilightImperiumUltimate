namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class NewTiglUserResponse
{
    public bool Success { get; set; }

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
