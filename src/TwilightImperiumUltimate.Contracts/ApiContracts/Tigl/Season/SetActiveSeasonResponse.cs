namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class SetActiveSeasonResponse
{
    public bool Success { get; set; }

    public int SeasonNumber { get; set; }

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
