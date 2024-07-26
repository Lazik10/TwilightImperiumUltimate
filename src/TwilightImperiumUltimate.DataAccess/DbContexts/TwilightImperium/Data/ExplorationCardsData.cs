namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ExplorationCardsData
{
    internal static List<ExplorationCard> ExplorationCards => new List<ExplorationCard>
    {
        // Cultural
        new() { EnumName = ExplorationCardName.CulturalRelicFragment, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.DemilitarizedZone, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.DysonSphere, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.Freelancers, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.GammaWormhole, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.MercenaryOutfit, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.ParadiseWorld, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.TombOfEmphidia, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },

        new() { EnumName = ExplorationCardName.CouncilPreserve, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.DesertedTradeStation, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.DistinguishedAdmiral, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.StarChartCultural, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },

        // Hazardous
        new() { EnumName = ExplorationCardName.CoreMine, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.Expedition, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.HazardousRelicFragment, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.LazaxSurvivors, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.MiningWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.RichWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.VolatileFuelSource, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.WarfareResearchFacility, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },

        new() { EnumName = ExplorationCardName.ArcaneCitadel, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.ScorchedDepot, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.SeedySpaceport, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.StarChartHazardous, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },

        // Industrial
        new() { EnumName = ExplorationCardName.AbandonedWarehouses, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.BioticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.CyberneticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.FunctioningBase, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.IndustrialRelicFragment, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.LocalFabricators, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { EnumName = ExplorationCardName.PropulsionResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },

        new() { EnumName = ExplorationCardName.AncientShipyard, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.HiddenLab, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.OrbitalFoundries, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { EnumName = ExplorationCardName.StarChartIndustrial, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
    };
}
