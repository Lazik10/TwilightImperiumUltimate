using TwilightImperiumUltimate.Core.Enums.Races;

namespace TwilightImperiumUltimate.Draft.Draft.FactionDraft;

public class DraftResult
{
    public int PlayerId { get; set; }

    public IEnumerable<FactionName> Factions { get; set; } = new List<FactionName>();
}
