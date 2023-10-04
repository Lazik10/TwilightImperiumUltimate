using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;

namespace TwilightImperiumUltimate.Core.Models.Factions;

public class ColorDraftRequest
{
    public IReadOnlyCollection<FactionName> Factions { get; set; } = new List<FactionName>();

    public IReadOnlyCollection<PlayerColor> Colors { get; set; } = new List<PlayerColor>();
}
