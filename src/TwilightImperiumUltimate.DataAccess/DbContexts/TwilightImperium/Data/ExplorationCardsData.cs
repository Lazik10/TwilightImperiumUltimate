namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ExplorationCardsData
{
    internal static List<ExplorationCard> ExplorationCards => new List<ExplorationCard>
    {
        // Cultural
        new() { Id = 1, EnumName = ExplorationCardName.CulturalRelicFragment, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 2, EnumName = ExplorationCardName.DemilitarizedZone, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 3, EnumName = ExplorationCardName.DysonSphere, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 4, EnumName = ExplorationCardName.Freelancers, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 5, EnumName = ExplorationCardName.GammaWormhole, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 6, EnumName = ExplorationCardName.MercenaryOutfit, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 7, EnumName = ExplorationCardName.ParadiseWorld, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 8, EnumName = ExplorationCardName.TombOfEmphidia, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.ProphecyOfKings },

        new() { Id = 9, EnumName = ExplorationCardName.CouncilPreserve, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 10, EnumName = ExplorationCardName.DesertedTradeStation, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 11, EnumName = ExplorationCardName.DistinguishedAdmiral, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 12, EnumName = ExplorationCardName.StarChartCultural, ExplorationPlanetTrait = PlanetTrait.Cultural, GameVersion = GameVersion.UnchartedSpace },

        // Hazardous
        new() { Id = 13, EnumName = ExplorationCardName.CoreMine, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 14, EnumName = ExplorationCardName.Expedition, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 15, EnumName = ExplorationCardName.HazardousRelicFragment, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 16, EnumName = ExplorationCardName.LazaxSurvivors, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 17, EnumName = ExplorationCardName.MiningWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 18, EnumName = ExplorationCardName.RichWorld, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 19, EnumName = ExplorationCardName.VolatileFuelSource, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 20, EnumName = ExplorationCardName.WarfareResearchFacility, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.ProphecyOfKings },

        new() { Id = 21, EnumName = ExplorationCardName.ArcaneCitadel, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 22, EnumName = ExplorationCardName.ScorchedDepot, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 23, EnumName = ExplorationCardName.SeedySpaceport, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 24, EnumName = ExplorationCardName.StarChartHazardous, ExplorationPlanetTrait = PlanetTrait.Hazardous, GameVersion = GameVersion.UnchartedSpace },

        // Industrial
        new() { Id = 25, EnumName = ExplorationCardName.AbandonedWarehouses, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 26, EnumName = ExplorationCardName.BioticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 27, EnumName = ExplorationCardName.CyberneticResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 28, EnumName = ExplorationCardName.FunctioningBase, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 29, EnumName = ExplorationCardName.IndustrialRelicFragment, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 30, EnumName = ExplorationCardName.LocalFabricators, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },
        new() { Id = 31, EnumName = ExplorationCardName.PropulsionResearchFacility, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.ProphecyOfKings },

        new() { Id = 32, EnumName = ExplorationCardName.AncientShipyard, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 33, EnumName = ExplorationCardName.HiddenLab, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 34, EnumName = ExplorationCardName.OrbitalFoundries, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
        new() { Id = 35, EnumName = ExplorationCardName.StarChartIndustrial, ExplorationPlanetTrait = PlanetTrait.Industrial, GameVersion = GameVersion.UnchartedSpace },
    };
}
