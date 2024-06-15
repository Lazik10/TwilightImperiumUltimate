using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Technologies;

public class Technology : IEntity, IGameVersion
{
    public int Id { get; set; }

    public TechnologyName TechnologyName { get; set; }

    public TechnologyType Type { get; set; }

    public TechnologyLevel Level { get; set; }

    public bool IsFactionTechnology { get; set; }

    public FactionName FactionName { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public GameVersion GameVersion { get; set; }

    public IReadOnlyCollection<FactionTechnology> FactionTechnologies { get; set; } = new List<FactionTechnology>();
}
