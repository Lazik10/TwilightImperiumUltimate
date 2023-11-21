using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Units;

namespace TwilightImperiumUltimate.API.DTOs.Factions;

public class FactionDto
{
        public FactionName FactionName { get; set; }

        public int Commodities { get; set; }

        public int ComplexityRating { get; set; }

        public GameVersion GameVersion { get; set; }

        public IReadOnlyDictionary<UnitName, int> Units { get; set; } = new Dictionary<UnitName, int>();
}
