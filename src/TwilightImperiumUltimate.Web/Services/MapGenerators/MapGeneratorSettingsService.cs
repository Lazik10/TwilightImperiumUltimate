using TwilightImperiumUltimate.Web.Options.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorSettingsService(
    ITwilightImperiumApiHttpClient httpClient,
    IMapper mapper)
    : IMapGeneratorSettingsService
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;
    private readonly Random random = new();

    public List<FactionModel> FactionsForMapGenerator { get; set; } = new List<FactionModel>();

    public int MapScale { get; set; } = MapGeneratorOptions.DefaultScale;

    public MapTemplate MapTemplate { get; set; } = MapGeneratorOptions.MapTemplate;

    public PlacementStyle PlacementStyle { get; set; } = MapGeneratorOptions.PlacementStyle;

    public SystemWeight SystemWeight { get; set; } = MapGeneratorOptions.SystemWeight;

    public List<GameVersion> GameVersions { get; set; } = new List<GameVersion>() { GameVersion.BaseGame, GameVersion.ProphecyOfKings };

    public SystemTileOverlay SystemTileOverlay { get; set; } = MapGeneratorOptions.SystemTileOverlay;

    public WormholeDensity WormholeDensity { get; set; } = MapGeneratorOptions.WormholeDensity;

    public int NumberOfLegendaryPlanets { get; set; } = MapGeneratorOptions.NumberOfLegendaryPlanets;

    public bool LegendaryPriorityInEquidistant { get; set; } = MapGeneratorOptions.LegendaryPriorityInEquidistant;

    public bool EnableFactionPick { get; set; } = MapGeneratorOptions.EnableFactionPick;

    public List<MapGeneratorPlayerModel> Players { get; set; } = new List<MapGeneratorPlayerModel>();

    public bool EnablePlayerNames { get; set; } = MapGeneratorOptions.EnablePlayerNames;

    public void IncreaseMapScale()
    {
        if (MapScale <= MapGeneratorOptions.MaxScale - MapGeneratorOptions.ScaleIncrement)
            MapScale += MapGeneratorOptions.ScaleIncrement;
    }

    public void DecreaseMapScale()
    {
        if (MapScale >= MapGeneratorOptions.MinScale + MapGeneratorOptions.ScaleIncrement)
            MapScale -= MapGeneratorOptions.ScaleIncrement;
    }

    public void UpdateGameVersion(GameVersion gameVersion)
    {
        if (!GameVersions.Remove(gameVersion))
            GameVersions.Add(gameVersion);

        int maxNumberOfLegendaryPlanets = 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (NumberOfLegendaryPlanets > maxNumberOfLegendaryPlanets)
            NumberOfLegendaryPlanets = maxNumberOfLegendaryPlanets;
    }

    public void UpdateWormholeDensity(WormholeDensity wormholeDensity) => WormholeDensity = wormholeDensity;

    public void UpdateFactionBanStatus(FactionModel factionModel)
    {
        FactionsForMapGenerator.ForEach(x =>
        {
            if (x.FactionName == factionModel.FactionName && x.Banned)
                x.Banned = true;
            else if (x.FactionName == factionModel.FactionName && !x.Banned)
                x.Banned = false;
        });
    }

    public void GameVersionGlobalEnableDisable(GameVersion gameVersion)
    {
        if (FactionsForMapGenerator.Exists(x => x.GameVersion == gameVersion && !x.Banned))
        {
            FactionsForMapGenerator.ForEach(x =>
            {
                if (x.GameVersion == gameVersion)
                    x.Banned = true;
            });
        }
        else if (FactionsForMapGenerator.Where(x => x.GameVersion == gameVersion).All(x => x.Banned))
        {
            FactionsForMapGenerator.ForEach(x =>
            {
                if (x.GameVersion == gameVersion)
                    x.Banned = false;
            });
        }
    }

    public async Task InitializeFactionsForMapGenerator()
    {
        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions);
        if (statusCode == HttpStatusCode.OK)
        {
            FactionsForMapGenerator = _mapper.Map<List<FactionModel>>(response!.Data!.Items);
        }
    }

    public int GetMapTemplatePlayerCount()
    {
        return MapTemplate switch
        {
            MapTemplate.CustomMap => 0,

            MapTemplate.TwoPlayersMediumMap
            => 2,

            MapTemplate.ThreePlayersSmallMap or
            MapTemplate.ThreePlayersSmallAlternateMap or
            MapTemplate.ThreePlayersMediumHyperlineMap or
            MapTemplate.ThreePlayersMediumTriangleMap or
            MapTemplate.ThreePlayersMediumTriangleNarrowMap or
            MapTemplate.ThreePlayersMediumSnowflakeMap or
            MapTemplate.ThreePlayersMediumMantaRayMap or
            MapTemplate.ThreePlayersMediumTridentMap or
            MapTemplate.ThreePlayersMediumRexMap
            => 3,

            MapTemplate.FourPlayersMediumMap or
            MapTemplate.FourPlayersMediumHorizontalMap or
            MapTemplate.FourPlayersMediumHyperlineMap or
            MapTemplate.FourPlayersMediumVerticalMap or
            MapTemplate.FourPlayersMediumGapsMap or
            MapTemplate.FourPlayersMediumWarpMap or
            MapTemplate.FourPlayersMediumMiniWarpMap or
            MapTemplate.FourPlayersMediumDoubleWarpMap
            => 4,

            MapTemplate.FivePlayersMediumMap or
            MapTemplate.FivePlayersMediumHyperlineMap or
            MapTemplate.FivePlayersMediumDiamondMap or
            MapTemplate.FivePlayersLargeFlatMap
            => 5,

            MapTemplate.SixPlayersMediumMap or
            MapTemplate.SixPlayersLargeMap or
            MapTemplate.SixPlayersMediumSpiralMap
            => 6,

            MapTemplate.SevenPlayersLargeHyperlineMap or
            MapTemplate.SevenPlayersLargeWarpMap
            => 7,

            MapTemplate.EightPlayersLargeMap or
            MapTemplate.EightPlayersLargeWarpMap
            => 8,

            _ => 0,
        };
    }

    public Task InitializePlayersForMapGenerator()
    {
        if (Players.Count == 0 || Players.Count != GetMapTemplatePlayerCount())
        {
            Players = new List<MapGeneratorPlayerModel>();
            for (int i = 0; i < GetMapTemplatePlayerCount(); i++)
            {
                Players.Add(new MapGeneratorPlayerModel());
            }
        }

        return Task.CompletedTask;
    }

    public IReadOnlyCollection<string> GetPlayerNames()
    {
        return Players.OrderBy(x => random.Next()).Select(x => x.PlayerName).ToList();
    }
}
