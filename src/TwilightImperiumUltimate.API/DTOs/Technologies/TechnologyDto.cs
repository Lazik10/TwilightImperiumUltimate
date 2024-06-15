using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;

namespace TwilightImperiumUltimate.API.DTOs.Technologies;

public class TechnologyDto
{
    public int Id { get; set; }

    public TechnologyName TechnologyName { get; set; }

    public TechnologyType Type { get; set; }

    public TechnologyLevel Level { get; set; }

    public bool IsFactionTechnology { get; set; }

    public FactionName FactionName { get; set; }

    public GameVersion GameVersion { get; set; }
}
