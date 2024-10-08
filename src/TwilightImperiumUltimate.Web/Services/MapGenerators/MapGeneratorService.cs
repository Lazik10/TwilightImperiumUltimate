using TwilightImperiumUltimate.Contracts.ApiContracts.Draft;
using TwilightImperiumUltimate.Contracts.DTOs.MapGeneration;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorService(
    IMapGeneratorSettingsService mapGeneratorSettingsService,
    ITwilightImperiumApiHttpClient twilightImperiumApiHttpClient,
    IMapper mapper)
    : IMapGeneratorService
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService = mapGeneratorSettingsService;
    private readonly ITwilightImperiumApiHttpClient _httpClient = twilightImperiumApiHttpClient;
    private readonly IMapper _mapper = mapper;
    private Dictionary<int, SystemTileModel> _systemTiles = new Dictionary<int, SystemTileModel>();

    public SystemTileModel DraggedSystemTile { get; set; } = new();

    public int DraggedSystemTileStartMapPosition { get; set; }

    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles => _systemTiles;

    public IEnumerable<SystemTileModel> AllSystemTiles { get; set; } = new List<SystemTileModel>();

    public async Task<Dictionary<int, SystemTileModel>> GenerateMapAsync(bool previewMap, CancellationToken ct)
    {
        GenerateMapRequest request = new(
            previewMap,
            _mapGeneratorSettingsService.MapTemplate,
            _mapGeneratorSettingsService.EnableFactionPick ? HomeSystemDraftType.SpecificFactions : HomeSystemDraftType.Placeholders,
            _mapGeneratorSettingsService.FactionsForMapGenerator.Where(x => !x.Banned).Select(x => x.FactionName).ToList(),
            _mapGeneratorSettingsService.GameVersions,
            _mapGeneratorSettingsService.PlacementStyle,
            _mapGeneratorSettingsService.SystemWeight,
            _mapGeneratorSettingsService.WormholeDensity,
            _mapGeneratorSettingsService.NumberOfLegendaryPlanets,
            _mapGeneratorSettingsService.LegendaryPriorityInEquidistant,
            _mapGeneratorSettingsService.GetPlayerNames());

        var (response, statusCode) = await _httpClient.PostAsync<GenerateMapRequest, ApiResponse<GeneratedMapLayoutDto>>(Paths.ApiPath_GenerateMap, request, ct);

        var sortedMapLayoutDictionary = new Dictionary<int, SystemTileModel>();

        if (statusCode == HttpStatusCode.OK)
        {
            var generatedMapLayout = response!.Data!.MapLayout.ToList();
            var sortedMapLayout = generatedMapLayout
                .GroupBy(hex => hex.X / 2) // Group by pairs (0,1), (2,3), (4,5), (6,7)
                .OrderBy(group => group.Key) // Ensure groups are ordered
                .SelectMany(group => group.OrderBy(hex => hex.Y)) // Sort each group by Y and flatten the groups
                .ToList();

            int index = 0;
            foreach (var hexDto in sortedMapLayout)
            {
                sortedMapLayoutDictionary.Add(index, _mapper.Map<SystemTileModel>(hexDto.SystemTile is null ? new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty } : hexDto.SystemTile));
                index++;
            }

            await _mapGeneratorSettingsService.InitializePlayersForMapGenerator();
            var playerNames = _mapGeneratorSettingsService.GetPlayerNames().ToList();

            foreach (var systemTile in sortedMapLayoutDictionary.Values)
            {
                if ((systemTile.SystemTileCategory == SystemTileCategory.Green && systemTile.FactionName != FactionName.None)
                    || systemTile.SystemTileName == SystemTileName.TileHome)
                {
                    systemTile.PlayerName = playerNames[0];
                    playerNames.RemoveAt(0);
                }
            }

            _systemTiles = sortedMapLayoutDictionary;
        }

        return sortedMapLayoutDictionary;
    }

    public async Task<Dictionary<int, SystemTileModel>> GenerateMapLayoutPreview(MapTemplate mapTemplate)
    {
        GenerateMapRequest request = new(
            true,
            mapTemplate,
            HomeSystemDraftType.Placeholders,
            new List<FactionName>(),
            new List<GameVersion>(),
            PlacementStyle.Random,
            SystemWeight.Random,
            WormholeDensity.Random,
            0,
            false,
            new List<string>());

        var (response, statusCode) = await _httpClient.PostAsync<GenerateMapRequest, ApiResponse<GeneratedMapLayoutDto>>(Paths.ApiPath_GenerateMap, request, default);

        var sortedMapLayoutDictionary = new Dictionary<int, SystemTileModel>();

        if (statusCode == HttpStatusCode.OK)
        {
            var generatedMapLayout = response!.Data!.MapLayout.ToList();
            var sortedMapLayout = generatedMapLayout
                .GroupBy(hex => hex.X / 2) // Group by pairs (0,1), (2,3), (4,5), (6,7)
                .OrderBy(group => group.Key) // Ensure groups are ordered
                .SelectMany(group => group.OrderBy(hex => hex.Y)) // Sort each group by Y and flatten the groups
                .ToList();

            int index = 0;
            foreach (var hexDto in sortedMapLayout)
            {
                sortedMapLayoutDictionary.Add(index, _mapper.Map<SystemTileModel>(hexDto.SystemTile is null ? new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty } : hexDto.SystemTile));
                index++;
            }
        }

        return sortedMapLayoutDictionary;
    }

    public async Task InitializeSystemTilesAsync(CancellationToken ct)
    {
        if (_systemTiles.Count == 0)
            _systemTiles = await GenerateMapAsync(true, ct);

        if (!AllSystemTiles.Any())
            await InitializeSystemTilesForMenu();
    }

    public IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType)
    {
        return systemTileType switch
        {
            SystemTileTypeFilter.Unused => AllSystemTiles.Where(x => !_systemTiles.Values.Any(y => y.SystemTileName == x.SystemTileName)).OrderBy(x => x.GameVersion),
            SystemTileTypeFilter.All => AllSystemTiles.OrderBy(x => x.GameVersion),
            SystemTileTypeFilter.MecatolRex => AllSystemTiles.Where(x => x.SystemTileName == SystemTileName.Tile18),
            SystemTileTypeFilter.HomeSystems => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Green),
            SystemTileTypeFilter.BlueTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Blue),
            SystemTileTypeFilter.RedTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Red),
            SystemTileTypeFilter.Hyperlanes => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Hyperlane),
            SystemTileTypeFilter.Legendary => AllSystemTiles.Where(x => x.Planets.Any(x => x.IsLegendary)),
            SystemTileTypeFilter.Wormholes => AllSystemTiles.Where(x => x.Wormholes.Count > 0),
            SystemTileTypeFilter.OnePlanet => AllSystemTiles.Where(x => x.Planets.Count == 1),
            SystemTileTypeFilter.TwoPlanets => AllSystemTiles.Where(x => x.Planets.Count == 2),
            SystemTileTypeFilter.ThreePlanets => AllSystemTiles.Where(x => x.Planets.Count == 3),
            SystemTileTypeFilter.Anomalies => AllSystemTiles.Where(x => x.AnomalyName != AnomalyName.None),
            SystemTileTypeFilter.Empty => AllSystemTiles.Where(x => !x.HasPlanets
                && x.GameVersion != GameVersion.Custom
                && x.SystemTileCategory != SystemTileCategory.Hyperlane
                && x.FactionName == FactionName.None),
            SystemTileTypeFilter.BaseGame => AllSystemTiles.Where(x => x.GameVersion == GameVersion.BaseGame),
            SystemTileTypeFilter.ProphecyOfKings => AllSystemTiles.Where(x => x.GameVersion == GameVersion.ProphecyOfKings),
            SystemTileTypeFilter.DiscordantStars => AllSystemTiles.Where(x => x.GameVersion == GameVersion.DiscordantStars),
            SystemTileTypeFilter.UnchartedSpace => AllSystemTiles.Where(x => x.GameVersion == GameVersion.UnchartedSpace),
            SystemTileTypeFilter.AscendantSun => AllSystemTiles.Where(x => x.GameVersion == GameVersion.AscendantSun),
            SystemTileTypeFilter.Custom => AllSystemTiles.Where(x => x.GameVersion == GameVersion.Custom),
            _ => throw new ArgumentOutOfRangeException(nameof(systemTileType), systemTileType, null),
        };
    }

    public void SetDraggingSystemTile(SystemTileModel systemTile)
    {
        DraggedSystemTile = systemTile.Copy();
    }

    public void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition)
    {
        DraggedSystemTileStartMapPosition = draggedSystemTileStartMapPosition;
    }

    public void ResetDraggingSystemTile(SystemTileModel systemTile)
    {
        DraggedSystemTile = new();
    }

    public SystemTileModel GetCurrentDraggingSystemTile()
    {
        return DraggedSystemTile;
    }

    public void SwapSystemTiles(SystemTileModel systemTile, int mapPosition)
    {
        if (systemTile is not null && DraggedSystemTileStartMapPosition != -1)
        {
            var draggedSystemTileToSwap = _systemTiles[DraggedSystemTileStartMapPosition];
            _systemTiles[DraggedSystemTileStartMapPosition] = systemTile;
            _systemTiles[mapPosition] = draggedSystemTileToSwap;
        }
        else
        {
            _systemTiles[mapPosition] = DraggedSystemTile;
        }
    }

    public string GetMapString()
    {
        var mapHashString = string.Empty;
        GeneratedPositionsWithSystemTiles.Values.ToList().ForEach(x => mapHashString += x.SystemTileName.ToString());
        return mapHashString;
    }

    public Task InitializeMapFromLink(Dictionary<int, SystemTileModel> map)
    {
        _systemTiles = map;
        return Task.CompletedTask;
    }

    private async Task InitializeSystemTilesForMenu()
    {
        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
            AllSystemTiles = _mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
    }
}
