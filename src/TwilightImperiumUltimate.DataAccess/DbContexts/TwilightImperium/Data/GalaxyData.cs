namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class GalaxyData
{
    internal static List<SystemTile> SystemTiles => new List<SystemTile>
    {
        new()
        {
            SystemTileName = SystemTileName.TileHome,
            SystemTileCode = "Home",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.None,
            Planets = [],
            Wormholes = [],
            GameVersion = GameVersion.Custom,
        },
        new()
        {
            SystemTileName = SystemTileName.TileEmpty,
            SystemTileCode = "Empty",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.None,
            Planets = [],
            Wormholes = [],
            GameVersion = GameVersion.Custom,
        },

        // Twilight Imperium 4th edition tiles
        new()
        {
            SystemTileName = SystemTileName.Tile01,
            SystemTileCode = "1",
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
            SystemTileCode = "2",
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
            SystemTileCode = "3",
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
            SystemTileCode = "4",
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
            SystemTileCode = "5",
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
            SystemTileCode = "6",
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
            SystemTileCode = "7",
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
            SystemTileCode = "8",
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
            SystemTileCode = "9",
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
            SystemTileCode = "10",
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
            SystemTileCode = "11",
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
            SystemTileCode = "12",
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
            SystemTileCode = "13",
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
            SystemTileCode = "14",
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
            SystemTileCode = "15",
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
            SystemTileCode = "16",
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
            SystemTileCode = "17",
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
            SystemTileCode = "18",
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
            SystemTileCode = "19",
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
            SystemTileCode = "20",
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
            SystemTileCode = "21",
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
            SystemTileCode = "22",
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
            SystemTileCode = "23",
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
            SystemTileCode = "24",
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
            SystemTileCode = "25",
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
            SystemTileCode = "26",
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
            SystemTileCode = "27",
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
            SystemTileCode = "28",
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
            SystemTileCode = "29",
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
            SystemTileCode = "30",
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
            SystemTileCode = "31",
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
            SystemTileCode = "32",
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
            SystemTileCode = "33",
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
            SystemTileCode = "34",
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
            SystemTileCode = "35",
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
            SystemTileCode = "36",
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
            SystemTileCode = "37",
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
            SystemTileCode = "38",
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
            SystemTileCode = "39",
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
            SystemTileCode = "40",
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
            SystemTileCode = "41",
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
            SystemTileCode = "42",
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
            SystemTileCode = "43",
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
            SystemTileCode = "44",
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
            SystemTileCode = "45",
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
            SystemTileCode = "46",
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
            SystemTileCode = "47",
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
            SystemTileCode = "48",
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
            SystemTileCode = "49",
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
            SystemTileCode = "50",
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
            SystemTileCode = "51",
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
            SystemTileCode = "52",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile53,
            SystemTileCode = "53",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile54,
            SystemTileCode = "54",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile55,
            SystemTileCode = "55",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile56,
            SystemTileCode = "56",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile57,
            SystemTileCode = "57",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile58,
            SystemTileCode = "58",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile59,
            SystemTileCode = "59",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile60,
            SystemTileCode = "60",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile61,
            SystemTileCode = "61",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile62,
            SystemTileCode = "62",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile63,
            SystemTileCode = "63",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile64,
            SystemTileCode = "64",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile65,
            SystemTileCode = "65",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile66,
            SystemTileCode = "66",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile67,
            SystemTileCode = "67",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile68,
            SystemTileCode = "68",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile69,
            SystemTileCode = "69",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile70,
            SystemTileCode = "70",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile71,
            SystemTileCode = "71",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile72,
            SystemTileCode = "72",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile73,
            SystemTileCode = "73",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile74,
            SystemTileCode = "74",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile75,
            SystemTileCode = "75",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile76,
            SystemTileCode = "76",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile77,
            SystemTileCode = "77",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile78,
            SystemTileCode = "78",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile79,
            SystemTileCode = "79",
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
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile80,
            SystemTileCode = "80",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.Supernova,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile81,
            SystemTileCode = "81",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.Supernova,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile82A,
            SystemTileCode = "82A",
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
                    GameVersion = GameVersion.ProphecyOfKings,
                },
            ],
            Wormholes =
            [
                new Wormhole()
                {
                    SystemTileName = SystemTileName.Tile82A,
                    WormholeName = WormholeName.Gamma,
                },
            ],
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile82B,
            SystemTileCode = "82B",
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
                    GameVersion = GameVersion.ProphecyOfKings,
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
                    WormholeName = WormholeName.Gamma,
                },
            ],
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile83A,
            SystemTileCode = "83A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile83B,
            SystemTileCode = "83B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile84A,
            SystemTileCode = "84A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile84B,
            SystemTileCode = "84B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile85A,
            SystemTileCode = "85A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile85B,
            SystemTileCode = "85B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile86A,
            SystemTileCode = "86A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile86B,
            SystemTileCode = "86B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile87A,
            SystemTileCode = "87A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile87B,
            SystemTileCode = "87B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile88A,
            SystemTileCode = "88A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile88B,
            SystemTileCode = "88B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile89A,
            SystemTileCode = "89A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile89B,
            SystemTileCode = "89B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile90A,
            SystemTileCode = "90A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile90B,
            SystemTileCode = "90B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile91A,
            SystemTileCode = "91A",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },
        new()
        {
            SystemTileName = SystemTileName.Tile91B,
            SystemTileCode = "91B",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Hyperlane,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.ProphecyOfKings,
        },

        new()
        {
            SystemTileName = SystemTileName.Tile92,
            SystemTileCode = "92",
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
            SystemTileCode = "93",
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
            SystemTileCode = "94",
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
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1001,
            SystemTileCode = "1001",
            FactionName = FactionName.TheAugursOfIlyxum,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Demis,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1001,
                    Resources = 2,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Chrion,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1001,
                    Resources = 2,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1002,
            SystemTileCode = "1002",
            FactionName = FactionName.TheBentorConglomerate,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Benc,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1002,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Hau,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1002,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1003,
            SystemTileCode = "1003",
            FactionName = FactionName.TheBerserkersOfKjalengard,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Kjalengard,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1003,
                    Resources = 3,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Hulgade,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1003,
                    Resources = 1,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1004,
            SystemTileCode = "1004",
            FactionName = FactionName.TheCeldauriTradeConfederation,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Louk,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1004,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Auldane,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1004,
                    Resources = 1,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1005,
            SystemTileCode = "1005",
            FactionName = FactionName.TheCheiranHordes,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Arche,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1005,
                    Resources = 2,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.GghurnTheta,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1005,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1006,
            SystemTileCode = "1006",
            FactionName = FactionName.TheDihMohnFlotilla,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Abyssus,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1006,
                    Resources = 4,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1007,
            SystemTileCode = "1007",
            FactionName = FactionName.TheEdynMandate,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Ekko,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1007,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Edyn,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1007,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Okke,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1007,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1008,
            SystemTileCode = "1008",
            FactionName = FactionName.TheFlorzenProfiteers,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Delmor,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1008,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Kyd,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1008,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1009,
            SystemTileCode = "1009",
            FactionName = FactionName.TheFreeSystemsCompact,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Kroll,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1009,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Idyn,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1009,
                    Resources = 1,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Cyrra,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1009,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1010,
            SystemTileCode = "1010",
            FactionName = FactionName.TheGheminaRaiders,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Drah,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1010,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Trykk,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1010,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1011,
            SystemTileCode = "1011",
            FactionName = FactionName.TheGhotiWayfarers,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Ghoti,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1011,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1012,
            SystemTileCode = "1012",
            FactionName = FactionName.TheGledgeUnion,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.LastStop,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1012,
                    Resources = 3,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1013,
            SystemTileCode = "1013",
            FactionName = FactionName.TheGlimmerOfMortheus,
            Anomaly = AnomalyName.Nebula,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Biaheo,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1013,
                    Resources = 3,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Empero,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1013,
                    Resources = 0,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1014,
            SystemTileCode = "1014",
            FactionName = FactionName.TheKolleccSociety,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Susuros,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1014,
                    Resources = 4,
                    Influence = 4,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1015,
            SystemTileCode = "1015",
            FactionName = FactionName.TheKortaliTribunal,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Ogdun,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1015,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Brthkul,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1015,
                    Resources = 1,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1016,
            SystemTileCode = "1016",
            FactionName = FactionName.TheKyroSodality,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Avicenna,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1016,
                    Resources = 4,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1017,
            SystemTileCode = "1017",
            FactionName = FactionName.TheLanefirRemnants,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.AysisRest,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1017,
                    Resources = 4,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Solitude,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1017,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1018,
            SystemTileCode = "1018",
            FactionName = FactionName.TheLiZhoDynasty,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Pax,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1018,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Kyr,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1018,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Vess,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1018,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1019,
            SystemTileCode = "1019",
            FactionName = FactionName.TheLTokkKhrask,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.BohlDhur,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1019,
                    Resources = 3,
                    Influence = 4,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1020,
            SystemTileCode = "1020",
            FactionName = FactionName.TheMirvedaProtectorate,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Aldra,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1020,
                    Resources = 2,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Beata,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1020,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1021,
            SystemTileCode = "1021",
            FactionName = FactionName.TheMonksOfKolume,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Alesna,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1021,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Azle,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1021,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1022,
            SystemTileCode = "1022",
            FactionName = FactionName.TheMykoMentori,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.ShiHalaum,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1022,
                    Resources = 4,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1023,
            SystemTileCode = "1023",
            FactionName = FactionName.TheNivynStarKings,
            Anomaly = AnomalyName.GravityRift,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Ellas,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1023,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1024,
            SystemTileCode = "1024",
            FactionName = FactionName.TheNokarSellships,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Zarr,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1024,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Nokk,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1024,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1025,
            SystemTileCode = "1025",
            FactionName = FactionName.TheOlradinLeague,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Sanctuary,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1025,
                    Resources = 3,
                    Influence = 4,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1026,
            SystemTileCode = "1026",
            FactionName = FactionName.RohDhnaMechatronics,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Prind,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1026,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1027,
            SystemTileCode = "1027",
            FactionName = FactionName.TheSavagesOfCymiae,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Cymiae,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1027,
                    Resources = 3,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1028,
            SystemTileCode = "1028",
            FactionName = FactionName.TheShipwrightsofAxis,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Axis,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1028,
                    Resources = 5,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1029,
            SystemTileCode = "1029",
            FactionName = FactionName.TheTnelisSyndicate,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Discordia,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1029,
                    Resources = 4,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1030,
            SystemTileCode = "1030",
            FactionName = FactionName.TheVadenBankingClans,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Vadarian,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1030,
                    Resources = 3,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Norvus,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1030,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1031,
            SystemTileCode = "1031",
            FactionName = FactionName.TheVaylerianScourge,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Vaylar,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1031,
                    Resources = 3,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1032,
            SystemTileCode = "1032",
            FactionName = FactionName.TheVeldyrSovereignty,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Rhune,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1032,
                    Resources = 3,
                    Influence = 4,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1033,
            SystemTileCode = "1033",
            FactionName = FactionName.TheZealotsOfRhodun,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Poh,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1033,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Orad,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1033,
                    Resources = 3,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1034,
            SystemTileCode = "1034",
            FactionName = FactionName.TheZelianPurifier,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Green,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Gen,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1034,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Zelian,
                    PlanetTrait = PlanetTrait.None,
                    SystemTileName = SystemTileName.Tile1034,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.DiscordantStars,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1035A,
            SystemTileCode = "1035A",
            FactionName = FactionName.TheMykoMentori,
            Anomaly = AnomalyName.AsteroidField,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1035B,
            SystemTileCode = "1035B",
            FactionName = FactionName.TheMykoMentori,
            Anomaly = AnomalyName.Nebula,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile1036,
            SystemTileCode = "1036",
            FactionName = FactionName.TheZelianPurifier,
            Anomaly = AnomalyName.AsteroidField,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.DiscordantStars,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4253,
            SystemTileCode = "4253",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Silence,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4253,
                    Resources = 2,
                    Influence = 2,
                    IsLegendary = true,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4254,
            SystemTileCode = "4254",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Echo,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4254,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = true,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4255,
            SystemTileCode = "4255",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Tarrock,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4255,
                    Resources = 3,
                    Influence = 0,
                    IsLegendary = true,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4256,
            SystemTileCode = "4256",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Prism,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4256,
                    Resources = 0,
                    Influence = 3,
                    IsLegendary = true,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4257,
            SystemTileCode = "4257",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Troac,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4257,
                    Resources = 0,
                    Influence = 4,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4258,
            SystemTileCode = "4258",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.EtirV,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4258,
                    Resources = 4,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4259,
            SystemTileCode = "4259",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Vioss,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4259,
                    Resources = 3,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4260,
            SystemTileCode = "4260",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Fakrenn,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4260,
                    Resources = 2,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes =
            [
                new Wormhole()
                {
                    WormholeName = WormholeName.Alpha,
                    SystemTileName = SystemTileName.Tile4260,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4261,
            SystemTileCode = "4261",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.SanVit,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4261,
                    Resources = 3,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Lodran,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4261,
                    Resources = 0,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.Cybernetic,
                    GameVersion = GameVersion.UnchartedSpace,
                }

            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4262,
            SystemTileCode = "4262",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Dorvok,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4262,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.Warfare,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Derbrae,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4262,
                    Resources = 2,
                    Influence = 3,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4263,
            SystemTileCode = "4263",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Rysaa,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4263,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.Propulsion,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Moln,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4263,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.Biotic,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4264,
            SystemTileCode = "4264",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Salin,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4264,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Gwiyun,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4264,
                    Resources = 2,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4265,
            SystemTileCode = "4265",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Inan,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4265,
                    Resources = 1,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Swog,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4265,
                    Resources = 1,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4266,
            SystemTileCode = "4266",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Detic,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4266,
                    Resources = 3,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Lliot,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4266,
                    Resources = 0,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4267,
            SystemTileCode = "4267",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Qaak,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4267,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Larred,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4267,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Nairb,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4267,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4268,
            SystemTileCode = "4268",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Blue,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Sierpen,
                    PlanetTrait = PlanetTrait.Cultural,
                    SystemTileName = SystemTileName.Tile4268,
                    Resources = 2,
                    Influence = 0,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Mandle,
                    PlanetTrait = PlanetTrait.Industrial,
                    SystemTileName = SystemTileName.Tile4268,
                    Resources = 1,
                    Influence = 1,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                new Planet()
                {
                    PlanetName = PlanetName.Regnem,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4268,
                    Resources = 0,
                    Influence = 2,
                    IsLegendary = false,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4269,
            SystemTileCode = "4269",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.Nebula,
            TileCategory = SystemTileCategory.Red,
            Planets =
            [
                new Planet()
                {
                    PlanetName = PlanetName.Domna,
                    PlanetTrait = PlanetTrait.Hazardous,
                    SystemTileName = SystemTileName.Tile4269,
                    Resources = 2,
                    Influence = 1,
                    IsLegendary = true,
                    TechnologySkip = TechnologyType.None,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4270,
            SystemTileCode = "4270",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4271,
            SystemTileCode = "4271",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.None,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4272,
            SystemTileCode = "4272",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.Nebula,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes =
            [
                new Wormhole()
                {
                    WormholeName = WormholeName.Beta,
                    SystemTileName = SystemTileName.Tile4272,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4273,
            SystemTileCode = "4273",
            FactionName = FactionName.None,

            // Also Nebula, but I would need to implement flags and it doesn't really matter
            // in druft, it is picked randomly from red tiles not based on AnomalyType
            Anomaly = AnomalyName.AsteroidField,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4274,
            SystemTileCode = "4274",
            FactionName = FactionName.None,

            // Also Nebula, but I would need to implement flags and it doesn't really matter
            // in druft, it is picked randomly from red tiles not based on AnomalyType
            Anomaly = AnomalyName.GravityRift,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes = default!,
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4275,
            SystemTileCode = "4275",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.GravityRift,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes =
            [
                new Wormhole()
                {
                    WormholeName = WormholeName.Gamma,
                    SystemTileName = SystemTileName.Tile4275,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            GameVersion = GameVersion.UnchartedSpace,
        },
        new SystemTile()
        {
            SystemTileName = SystemTileName.Tile4276,
            SystemTileCode = "4276",
            FactionName = FactionName.None,
            Anomaly = AnomalyName.Supernova,
            TileCategory = SystemTileCategory.Red,
            Planets = [],
            Wormholes =
            [
                new Wormhole()
                {
                    WormholeName = WormholeName.Alpha,
                    SystemTileName = SystemTileName.Tile4276,
                    GameVersion = GameVersion.UnchartedSpace,
                },
                    new Wormhole()
                {
                    WormholeName = WormholeName.Beta,
                    SystemTileName = SystemTileName.Tile4276,
                    GameVersion = GameVersion.UnchartedSpace,
                },
            ],
            GameVersion = GameVersion.UnchartedSpace,
        },
    };
}
