using TwilightImperiumUltimate.Web.Models.Factions;

namespace TwilightImperiumUltimate.Web.Models.Drafts;

public class FactionDraftRequest
{
    public int NumberOfPlayers { get; set; }

    public int NumberOfFactionsPerPlayer { get; set; }

    public IReadOnlyCollection<FactionModel> Factions { get; set; } = new List<FactionModel>();
}
