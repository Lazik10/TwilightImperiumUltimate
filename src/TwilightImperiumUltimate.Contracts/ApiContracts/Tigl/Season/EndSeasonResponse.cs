namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class EndSeasonResponse
{
    public bool Success { get; set; }

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
