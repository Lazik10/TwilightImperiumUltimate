using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Units;
using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Interfaces;
using TwilightImperiumUltimate.DataAccess.RelationshipEntities;

namespace TwilightImperiumUltimate.Core.Entities.Factions;

public class Faction : IEntity, IGameVersion
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets race Name.
    /// </summary>
    public FactionName FactionName { get; set; }

    /// <summary>
    /// Gets or sets race home system tile.
    /// </summary>
    public SystemTileName HomeSystem { get; set; }

    /// <summary>
    /// Gets or sets number of commodities at the start of the game.
    /// </summary>
    public int Commodities { get; set; }

    /// <summary>
    /// Gets or sets difficulty of the race.
    /// </summary>
    public ComplexityRating ComplexityRating { get; set; }

    /// <summary>
    /// Gets or sets actions overview.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets promissary note text.
    /// </summary>
    public string PromissaryNote { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets history about the race from Guide to Twilight Imperium.
    /// </summary>
    public string History { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets charactersistic quote from Guide to Twilight Imperium.
    /// </summary>
    public string Quote { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets system status.
    /// </summary>
    public string SystemStats { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets system info.
    /// </summary>
    public string SystemInfo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets origin of this race.
    /// </summary>
    public GameVersion GameVersion { get; set; }

    /// <summary>
    /// Gets or sets all technologies that can be researched by the race.
    /// </summary>
    public IReadOnlyCollection<FactionTechnology> FactionTechnologies { get; set; } = new List<FactionTechnology>();

    /// <summary>
    /// Gets or sets all starting units.
    /// </summary>
    public IReadOnlyCollection<FactionUnit> FactionUnits { get; set; } = new List<FactionUnit>();
}
