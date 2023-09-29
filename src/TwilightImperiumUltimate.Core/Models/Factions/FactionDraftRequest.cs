using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.API.Models.Factions;

public class FactionDraftRequest
{
    public int NumberOfPlayers { get; set; }

    public int NumberOfFactionsPerPlayer { get; set; }

    public IReadOnlyList<FactionModel> Factions { get; set; } = new List<FactionModel>();
}
