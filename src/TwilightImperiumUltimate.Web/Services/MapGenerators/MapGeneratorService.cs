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
        MapDraftRequest request = new()
        {
            Factions = new List<FactionName>(),
            MapTemplate = _mapGeneratorSettingsService.MapTemplate,
            PlacementStyle = _mapGeneratorSettingsService.PlacementStyle,
            SystemWeight = _mapGeneratorSettingsService.SystemWeight,
            PreviewMap = previewMap,
        };

        var (response, statusCode) = await _httpClient.PostAsync<MapDraftRequest, ApiResponse<GeneratedMapLayoutDto>> (Paths.ApiPath_GenerateMap, request, ct);

        if (statusCode == HttpStatusCode.OK)
            _systemTiles = _mapper.Map<Dictionary<int, SystemTileModel>>(response!.Data!.MapLayout);

        return _mapper.Map<Dictionary<int, SystemTileModel>>(response!.Data!.MapLayout);
    }

    public async Task InitializeSystemTilesAsync(CancellationToken ct)
    {
        _systemTiles = await GenerateMapAsync(true, ct);
        await InitializeSystemTilesForMenu();
    }

    public IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType)
    {
        return systemTileType switch
        {
            SystemTileTypeFilter.Unused => AllSystemTiles.Where(x => !_systemTiles.Values.Any(y => y.SystemTileName == x.SystemTileName)),
            SystemTileTypeFilter.All => AllSystemTiles,
            SystemTileTypeFilter.MecatolRex => AllSystemTiles.Where(x => x.SystemTileName == SystemTileName.Tile18),
            SystemTileTypeFilter.HomeSystems => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Green),
            SystemTileTypeFilter.BlueTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Blue),
            SystemTileTypeFilter.RedTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Red),
            SystemTileTypeFilter.Hyperlanes => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Hyperlane),
            _ => throw new ArgumentOutOfRangeException(nameof(systemTileType), systemTileType, null),
        };
    }

    public void SetDraggingSystemTile(SystemTileModel systemTile)
    {
        DraggedSystemTile = systemTile;
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

    private async Task InitializeSystemTilesForMenu()
    {
        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
            AllSystemTiles = _mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
    }
}
