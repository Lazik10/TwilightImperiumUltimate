namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeTiles()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var systemTiles = new List<SystemTile>()
        {
            new SystemTile()
            {
                SystemTileName = SystemTileName.TileHome,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.None,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>(),
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile()
            {
                SystemTileName = SystemTileName.TileEmpty,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.None,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>(),
                GameVersion = GameVersion.BaseGame,
            },

            // Twilight Imperium 4th edition tiles
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile01,
                RaceName = FactionName.TheFederationOfSol,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile02,
                RaceName = FactionName.TheMentakCoalition,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile03,
                RaceName = FactionName.TheYinBrotherhood,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile04,
                RaceName = FactionName.TheEmbersOfMuaat,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile05,
                RaceName = FactionName.TheArborec,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile06,
                RaceName = FactionName.TheL1z1xMindnet,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile07,
                RaceName = FactionName.TheWinnu,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile08,
                RaceName = FactionName.TheNekroVirus,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile09,
                RaceName = FactionName.TheNaaluCollective,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile10,
                RaceName = FactionName.TheBaronyOfLetnev,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile11,
                RaceName = FactionName.TheClanOfSaar,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile12,
                RaceName = FactionName.TheUniversitiesOfJolNar,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile13,
                RaceName = FactionName.SardakkNorr,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile14,
                RaceName = FactionName.TheXxchaKingdom,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile15,
                RaceName = FactionName.TheYssarilTribes,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                        PlanetName = PlanetName.Shalloo,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile15,
                        Resources = 1,
                        Influence = 2,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile16,
                RaceName = FactionName.TheEmiratesOfHacan,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile17,
                RaceName = FactionName.TheGhostsOfCreuss,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile17,
                        WormholeName = WormholeName.Delta,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile18,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.MecatolRex,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile19,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile20,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile21,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile22,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile23,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile24,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile25,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile25,
                        WormholeName = WormholeName.Beta,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile26,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile26,
                        WormholeName = WormholeName.Alpha,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile27,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile28,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile29,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile30,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile31,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile32,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile33,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile34,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile35,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile36,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile37,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile38,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile39,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile39,
                        WormholeName = WormholeName.Alpha,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile40,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile40,
                        WormholeName = WormholeName.Beta,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile41,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.GravityRift,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile42,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.Nebula,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile43,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile44,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile45,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile46,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile47,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile48,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile49,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile50,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.BaseGame,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile51,
                RaceName = FactionName.TheGhostsOfCreuss,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile51,
                        WormholeName = WormholeName.Delta,
                        GameVersion = GameVersion.BaseGame,
                    },
                },
                GameVersion = GameVersion.BaseGame,
            },

            // Prophecy of Kings expansion tiles
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile52,
                RaceName = FactionName.TheMahactGeneSorcerers,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile53,
                RaceName = FactionName.TheNomad,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile54,
                RaceName = FactionName.TheVuilRaithCabal,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile55,
                RaceName = FactionName.TheTitansOfUl,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile56,
                RaceName = FactionName.TheEmpyrean,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile57,
                RaceName = FactionName.TheNaazRokhaAlliance,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile58,
                RaceName = FactionName.TheArgentFlight,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile59,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile60,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile61,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile62,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile63,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile64,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile64,
                        WormholeName = WormholeName.Beta,
                    },
                },
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile65,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile66,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile67,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.GravityRift,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile68,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.Nebula,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile69,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile70,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile71,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile72,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile73,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile74,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile75,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile76,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile77,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile78,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Blue,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile79,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.AsteroidField,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile79,
                        WormholeName = WormholeName.Alpha,
                    },
                },
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile80,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.Supernova,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile81,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.Supernova,
                TileCategory = SystemTileCategory.Red,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile82A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets = new List<Planet>()
                {
                    new Planet()
                    {
                        PlanetName = PlanetName.MaliceFront,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile82A,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                },
                Wormholes = new List<Wormhole>()
                {
                    new Wormhole()
                    {
                        SystemTileName = SystemTileName.Tile82A,
                        WormholeName = WormholeName.Gama,
                    },
                },
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile82B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.ExternalMapTile,
                Planets = new List<Planet>()
                {
                    new Planet()
                    {
                        PlanetName = PlanetName.MaliceBack,
                        PlanetTrait = PlanetTrait.Cultural,
                        SystemTileName = SystemTileName.Tile82B,
                        Resources = 0,
                        Influence = 3,
                        IsLegendary = true,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.ProphecyOfKing,
                    },
                },
                Wormholes = new List<Wormhole>()
                {
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
                },
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile83A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile83B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile84A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile84B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile85A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile85B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile86A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile86B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile87A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile87B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile88A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile88B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile89A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile89B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile90A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile90B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile91A,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile91B,
                RaceName = FactionName.None,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Hyperlance,
                Planets = new List<Planet>(),
                Wormholes = default!,
                GameVersion = GameVersion.ProphecyOfKing,
            },

            new SystemTile
            {
                SystemTileName = SystemTileName.Tile92,
                RaceName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
                    new Planet()
                    {
                        PlanetName = PlanetName.MolPrimusCouncilOfKeleres,
                        PlanetTrait = PlanetTrait.None,
                        SystemTileName = SystemTileName.Tile92,
                        Resources = 4,
                        Influence = 1,
                        IsLegendary = false,
                        TechnologySkip = TechnologyType.None,
                        GameVersion = GameVersion.CodexVigil,
                    },
                },
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile93,
                RaceName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
            new SystemTile
            {
                SystemTileName = SystemTileName.Tile94,
                RaceName = FactionName.TheCouncilKeleres,
                Anomaly = AnomalyName.None,
                TileCategory = SystemTileCategory.Green,
                Planets = new List<Planet>()
                {
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
                },
                Wormholes = default!,
                GameVersion = GameVersion.CodexVigil,
            },
        };

        context.AddRange(systemTiles);
        context.SaveChanges();
    }
}
