namespace TwilightImperiumUltimate.Web.Models.GameTracker;

public class BonusObjectives
{
    public IReadOnlyCollection<AgendaCardName> AgendaCards { get; set; } = new List<AgendaCardName>();

    public IReadOnlyCollection<RelicCardName> Relics { get; set; } = new List<RelicCardName>();

    public IReadOnlyCollection<ActionCardName> ActionCards { get; set; } = new List<ActionCardName>();

    public bool CustodiansScored { get; set; }

    public int ImperialScore { get; set; }
}
