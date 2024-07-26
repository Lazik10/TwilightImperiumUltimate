using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class DraftResult
{
    public int PlayerId { get; set; }

    public IEnumerable<FactionName> Factions { get; set; } = [];
}
