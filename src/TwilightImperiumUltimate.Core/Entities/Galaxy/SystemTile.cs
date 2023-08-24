﻿using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Galaxy;

public class SystemTile : IEntity, IGameVersion
{
    public int Id { get; set; }

    public SystemTileName SystemTileName { get; set; }

    public SystemTileCategory TileCategory { get; set; }

    public FactionName RaceName { get; set; }

    public IReadOnlyCollection<Planet> Planets { get; set; } = new List<Planet>();

    public IReadOnlyCollection<Wormhole> Wormholes { get; set; } = new List<Wormhole>();

    public AnomalyName Anomaly { get; set; }

    public bool HasAnomaly => Anomaly != AnomalyName.None;

    public bool HasLegendaryPlanet => Planets.Any(x => x.IsLegendary);

    public bool IsHomeSystem => RaceName != FactionName.None;

    public GameVersion GameVersion { get; set; }
}
