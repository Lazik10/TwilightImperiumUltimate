using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;
using TwilightImperiumUltimate.Core.Enums.Units;

namespace TwilightImperiumUltimate.API.DTOs.Factions;

public class FactionDto
{
    public FactionName FactionName { get; set; }

    public int Commodities { get; set; }

    public int ComplexityRating { get; set; }

    public GameVersion GameVersion { get; set; }

    public IReadOnlyDictionary<UnitName, int> Units { get; set; } = new Dictionary<UnitName, int>();

    public IReadOnlyCollection<TechnologyName> FactionTechnologies { get; set; } = new List<TechnologyName>();

    public IReadOnlyCollection<PromissoryNoteName> PromissaryNotes { get; set; } = new List<PromissoryNoteName>();
}
