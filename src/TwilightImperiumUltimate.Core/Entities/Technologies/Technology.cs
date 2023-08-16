﻿using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Technologies;
using TwilightImperiumUltimate.Core.Interfaces;
using TwilightImperiumUltimate.DataAccess.RelationshipEntities;

namespace TwilightImperiumUltimate.Core.Entities.Technologies;

public class Technology : IEntity, IGameVersion
{
    public int Id { get; set; }

    public TechnologyName TechnologyName { get; set; }

    public TechnologyType Type { get; set; }

    public TechnologyLevel Level { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public GameVersion GameVersion { get; set; }

    public IReadOnlyCollection<RaceTechnology> RaceTechnologies { get; set; } = new List<RaceTechnology>();
}