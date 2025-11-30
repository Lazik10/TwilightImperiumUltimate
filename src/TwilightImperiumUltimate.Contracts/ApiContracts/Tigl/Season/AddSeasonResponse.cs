namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class AddSeasonResponse
{
    public bool Success { get; set; }

    public int SeasonNumber { get; set; }

    public string SeasonName { get; set; } = string.Empty;

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
