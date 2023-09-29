using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class DraftResult
{
    public int PlayerId { get; set; }

    public IEnumerable<FactionName> Factions { get; set; } = new List<FactionName>();
}
