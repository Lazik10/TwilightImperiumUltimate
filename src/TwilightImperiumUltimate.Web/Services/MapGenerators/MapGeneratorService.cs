using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Models.MapGenerators;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService;
    private readonly ITwilightImperiumApiHttpClient _twilightImperiumApiHttpClient;
    private Dictionary<int, SystemTile> _systemTiles = new Dictionary<int, SystemTile>();

    public MapGeneratorService(IMapGeneratorSettingsService mapGeneratorSettingsService, ITwilightImperiumApiHttpClient twilightImperiumApiHttpClient)
    {
        _mapGeneratorSettingsService = mapGeneratorSettingsService;
        _twilightImperiumApiHttpClient = twilightImperiumApiHttpClient;
    }

    public SystemTile DraggedSystemTile { get; set; } = new();

    public int DraggedSystemTileStartMapPosition { get; set; }

    public IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles => _systemTiles;

    public IEnumerable<SystemTile> AllSystemTiles { get; set; } = new List<SystemTile>();

    public async Task<Dictionary<int, SystemTile>> GenerateMapAsync(bool previewMap)
    {
        MapDraftRequest request = new()
        {
            Factions = new List<FactionName>(),
            MapTemplate = _mapGeneratorSettingsService.MapTemplate,
            PlacementStyle = _mapGeneratorSettingsService.PlacementStyle,
            SystemWeight = _mapGeneratorSettingsService.SystemWeight,
            PreviewMap = previewMap,
        };

        var result = await _twilightImperiumApiHttpClient.PostAsync<MapDraftRequest, MapDraftResult>(Paths.ApiPath_MapDraft, request);
        var systemTiles = result.MapTiles.ToDictionary();
        _systemTiles = systemTiles;

        return systemTiles;
    }

    public async Task InitializeSystemTilesAsync()
    {
        _systemTiles = await GenerateMapAsync(true);
        AllSystemTiles = await InitializeSystemTilesForMenu();
    }

    public IEnumerable<SystemTile> GetSystemTilesToShow(SystemTileTypeFilter systemTileType)
    {
        return systemTileType switch
        {
            SystemTileTypeFilter.Unused => AllSystemTiles.Where(x => !_systemTiles.Values.Any(y => y.Name == x.Name)),
            SystemTileTypeFilter.All => AllSystemTiles,
            SystemTileTypeFilter.MecatolRex => AllSystemTiles.Where(x => x.Name == SystemTileName.Tile18),
            SystemTileTypeFilter.HomeSystems => AllSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Green),
            SystemTileTypeFilter.BlueTiles => AllSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Blue),
            SystemTileTypeFilter.RedTiles => AllSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Red),
            SystemTileTypeFilter.Hyperlanes => AllSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Hyperlance),
            _ => throw new ArgumentOutOfRangeException(nameof(systemTileType), systemTileType, null),
        };
    }

    public void SetDraggingSystemTile(SystemTile systemTile)
    {
        DraggedSystemTile = systemTile;
    }

    public void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition)
    {
        DraggedSystemTileStartMapPosition = draggedSystemTileStartMapPosition;
    }

    public void ResetDraggingSystemTile(SystemTile systemTile)
    {
        DraggedSystemTile = new();
    }

    public SystemTile GetCurrentDraggingSystemTile()
    {
        return DraggedSystemTile;
    }

    public void SwapSystemTiles(SystemTile systemTile, int mapPosition)
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

    private async Task<List<SystemTile>> InitializeSystemTilesForMenu()
    {
        return await _twilightImperiumApiHttpClient.GetAsync<List<SystemTile>>(Paths.ApiPath_SystemTiles) ?? new List<SystemTile>();
    }
}
