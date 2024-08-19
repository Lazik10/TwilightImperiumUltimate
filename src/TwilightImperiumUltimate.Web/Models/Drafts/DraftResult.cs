namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class DraftResult
{
    public int PlayerId { get; set; }

    public IReadOnlyCollection<FactionName> Factions { get; set; } = new List<FactionName>();
}
