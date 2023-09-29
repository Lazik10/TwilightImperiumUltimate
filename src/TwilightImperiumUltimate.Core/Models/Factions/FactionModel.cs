using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;

namespace TwilightImperiumUltimate.Core.Models.Factions;

public class FactionModel
{
    public FactionName FactionName { get; set; }

    public int Commodities { get; set; }

    public int ComplexityRating { get; set; }

    public GameVersion GameVersion { get; set; }

    public bool Banned { get; set; }
}
