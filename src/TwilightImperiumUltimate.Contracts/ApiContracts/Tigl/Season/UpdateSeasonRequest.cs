namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class UpdateSeasonRequest
{
    public int SeasonNumber { get; set; }

    public string SeasonName { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
