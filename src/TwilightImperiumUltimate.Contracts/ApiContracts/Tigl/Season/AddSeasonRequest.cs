namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class AddSeasonRequest
{
    public int SeasonNumber { get; set; }

    public string SeasonName { get; set; } = string.Empty;
}
