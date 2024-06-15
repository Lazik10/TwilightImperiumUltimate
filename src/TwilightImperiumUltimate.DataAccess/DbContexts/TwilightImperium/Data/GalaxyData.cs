namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class GalaxyData
{
    internal static List<SystemTile> SystemTiles => GetSystemTiles();

    private static List<SystemTile> GetSystemTiles()
    {
        var systemTiles = new List<SystemTile>()
        {
            new()
            {
                SystemTileName = SystemTileName.TileHome,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.None,
                Planets = [],
                Wormholes = [],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.TileEmpty,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.None,
                Planets = [],
                Wormholes = [],
                GameVersion = GameVersion.BaseGame,
            },

            // Twilight Imperium 4th edition tiles
            new()
            {
                SystemTileName = SystemTileName.Tile01,
                FactionName = FactionName.TheFederationOfSol,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Jord,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile01,
                        Resources = 4,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile02,
                FactionName = FactionName.TheMentakCoalition,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MollPrimus,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile02,
                        Resources = 4,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile03,
                FactionName = FactionName.TheYinBrotherhood,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Darien,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile03,
                        Resources = 4,
                        Influence = 4,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile04,
                FactionName = FactionName.TheEmbersOfMuaat,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Muaat,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile04,
                        Resources = 4,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile05,
                FactionName = FactionName.TheArborec,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Nestphar,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile05,
                        Resources = 3,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile06,
                FactionName = FactionName.TheL1z1xMindnet,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.ZeroDotZeroDotZeroDot,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile06,
                        Resources = 5,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile07,
                FactionName = FactionName.TheWinnu,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Winnu,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile07,
                        Resources = 3,
                        Influence = 4,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile08,
                FactionName = FactionName.TheNekroVirus,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MordaiTwo,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile08,
                        Resources = 4,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile09,
                FactionName = FactionName.TheNaaluCollective,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Maaluuk,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile09,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Druaa,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile09,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile10,
                FactionName = FactionName.TheBaronyOfLetnev,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.ArcPrime,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile10,
                        Resources = 4,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.WrenTerra,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile10,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile11,
                FactionName = FactionName.TheClanOfSaar,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.LisisTwo,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile11,
                        Resources = 1,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Ragh,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile11,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile12,
                FactionName = FactionName.TheUniversitiesOfJolNar,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Nar,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile12,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Jol,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile12,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile13,
                FactionName = FactionName.SardakkNorr,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Trenlak,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile13,
                        Resources = 1,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Quinarra,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile13,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile14,
                FactionName = FactionName.TheXxchaKingdom,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.ArchonRen,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile14,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.ArchonTau,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile14,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile15,
                FactionName = FactionName.TheYssarilTribes,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Retillion,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile15,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Shalloq,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile15,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile16,
                FactionName = FactionName.TheEmiratesOfHacan,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Arretze,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile16,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Hercant,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile16,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Kamdorn,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile16,
                        Resources = 0,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile17,
                FactionName = FactionName.TheGhostsOfCreuss,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = [],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile17,
                        WormholeName = WormholeName.Delta,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile18,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.MecatolRex,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MecatolRex,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile18,
                        Resources = 1,
                        Influence = 6,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile19,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Wellon,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile19,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Cybernetic,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile20,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.VefutTwo,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile20,
                        Resources = 2,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile21,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Thibah,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile21,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Propulsion,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile22,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.TarMann,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile22,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Biotic,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile23,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Saudor,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile23,
                        Resources = 2,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile24,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MeharXull,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile24,
                        Resources = 1,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Warfare,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile25,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Quann,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile25,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile25,
                        WormholeName = WormholeName.Beta,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile26,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Lodor,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile26,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile26,
                        WormholeName = WormholeName.Alpha,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile27,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.NewAlbion,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile27,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Biotic,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Starpoint,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile27,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile28,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Tequran,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile28,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Torkan,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile28,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile29,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Qucenn,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile29,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Rarron,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile29,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile30,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Mellon,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile30,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Zohbat,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile30,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile31,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Lazar,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile31,
                        Resources = 1,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Cybernetic,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Sakulag,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile31,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile32,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.DalBootha,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile32,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Xxehan,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile32,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile33,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Corneeq,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile33,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Resculon,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile33,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile34,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Centauri,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile34,
                        Resources = 1,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Gral,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile34,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Propulsion,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile35,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Bereg,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile35,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.LirtaFour,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile35,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile36,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Arnor,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile36,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Lor,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile36,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile37,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Arinam,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile37,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Meer,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile37,
                        Resources = 0,
                        Influence = 4,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Warfare,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile38,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Abyz,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile38,
                        Resources = 3,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Fria,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile38,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile39,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile39,
                        WormholeName = WormholeName.Alpha,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile40,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile40,
                        WormholeName = WormholeName.Beta,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile41,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.GravityRift,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile42,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.Nebula,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile43,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile44,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile45,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile46,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile47,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile48,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile49,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile50,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile51,
                FactionName = FactionName.TheGhostsOfCreuss,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Creuss,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile51,
                        Resources = 4,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile51,
                        WormholeName = WormholeName.Delta,
                        GameVersion = GameVersion.BaseGame,
                    },
                ],
                GameVersion = GameVersion.BaseGame,
            },

            // Prophecy of Kings expansion tiles
            new()
            {
                SystemTileName = SystemTileName.Tile52,
                FactionName = FactionName.TheMahactGeneSorcerers,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Ixth,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile52,
                        Resources = 3,
                        Influence = 5,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile53,
                FactionName = FactionName.TheNomad,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Arcturus,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile53,
                        Resources = 4,
                        Influence = 4,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile54,
                FactionName = FactionName.TheVuilRaithCabal,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Acheron,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile54,
                        Resources = 4,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile55,
                FactionName = FactionName.TheTitansOfUl,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Elysium,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile55,
                        Resources = 4,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile56,
                FactionName = FactionName.TheEmpyrean,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.TheDark,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile56,
                        Resources = 3,
                        Influence = 4,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile57,
                FactionName = FactionName.TheNaazRokhaAlliance,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Naazir,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile57,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Rokha,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile57,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile58,
                FactionName = FactionName.TheArgentFlight,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Ylir,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile58,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Valk,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile58,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Avar,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile58,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile59,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.ArchonVail,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile59,
                        Resources = 1,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Propulsion,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile60,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Perimeter,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile60,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile61,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Ang,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile61,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Warfare,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile62,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Semlore,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile62,
                        Resources = 2,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Cybernetic,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile63,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Vorhal,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile63,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Biotic,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile64,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Atlas,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile64,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile64,
                        WormholeName = WormholeName.Beta,
                    },
                ],
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile65,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Primor,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile65,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile66,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.HopesEnd,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile66,
                        Resources = 3,
                        Influence = 0,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile67,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.GravityRift,
                TileCategory = SystemTileCategory.Red,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Cormund,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile67,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile68,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.Nebula,
                TileCategory = SystemTileCategory.Red,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Everra,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile68,
                        Resources = 3,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile69,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Accoen,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile69,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.JeolIr,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile69,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile70,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Kraag,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile70,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Siig,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile70,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile71,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Bakal,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile71,
                        Resources = 3,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.AlioPrima,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile71,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile72,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Lisis,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile72,
                        Resources = 2,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Velnor,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile72,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Warfare,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile73,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Cealdri,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile73,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Cybernetic,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Xanhact,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile73,
                        Resources = 0,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile74,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.VegaMajor,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile74,
                        Resources = 2,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.VegaMinor,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile74,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Propulsion,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile75,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Abaddon,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile75,
                        Resources = 1,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Loki,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile75,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.Ashtroth,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile75,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile76,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.RigelTwo,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile76,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.RigelThree,
                        PlanetTrait = PlanetTrait.Industrial,
                        SystemTileName = SystemTileName.Tile76,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.Biotic,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.RigelOne,
                        PlanetTrait = PlanetTrait.Hazardous,
                        SystemTileName = SystemTileName.Tile76,
                        Resources = 0,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile77,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile78,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile79,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile79,
                        WormholeName = WormholeName.Alpha,
                    },
                ],
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile80,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.Supernova,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile81,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.Supernova,
                TileCategory = SystemTileCategory.Red,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile82A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MalliceInactive,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile82A,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile82A,
                        WormholeName = WormholeName.Gama,
                    },
                ],
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile82B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.Mallice,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile82B,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                ],
                Wormholes =
                [
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile82B,
                        WormholeName = WormholeName.Alpha,
                    },
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile82B,
                        WormholeName = WormholeName.Beta,
                    },
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile82B,
                        WormholeName = WormholeName.Gama,
                    },
                ],
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile83A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile83B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile84A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile84B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile85A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile85B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile86A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile86B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile87A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile87B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile88A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile88B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile89A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile89B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile90A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile90B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile91A,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile91B,
                FactionName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlane,
                Planets = [],
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },

            new()
            {
                SystemTileName = SystemTileName.Tile92,
                FactionName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.MollPrimusCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile92,
                        Resources = 4,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile93,
                FactionName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.ArchonRenCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile93,
                        Resources = 2,
                        Influence = 3,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.ArchonTauCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile93,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
            new()
            {
                SystemTileName = SystemTileName.Tile94,
                FactionName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets =
                [
                    new Planet()
                    {
                        PlanetName = PlanetName.YlirCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile94,
                        Resources = 0,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.ValkCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile94,
                        Resources = 2,
                        Influence = 0,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                    new Planet()
                    {
                        PlanetName = PlanetName.AvarCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile94,
                        Resources = 1,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                ],
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
        };

        return systemTiles;
    }
}
