namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

public class SeasonDto
{
    public int SeasonNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
