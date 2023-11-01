using System.Net.Http.Json;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Models.MapGenerators;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly IMapGeneratorSettingsService _mapGeneratorSettingsService;
    private readonly HttpClient _http = new();
    private IReadOnlyCollection<SystemTile> _systemTiles = new List<SystemTile>();

    public MapGeneratorService(IMapGeneratorSettingsService mapGeneratorSettingsService)
    {
        _mapGeneratorSettingsService = mapGeneratorSettingsService;
    }

    public IReadOnlyCollection<SystemTile> SystemTiles => _systemTiles;

    public async Task<MapDraftResult> GenerateMapAsync()
    {
        Uri uri = new(Paths.ApiPath_MapDraft);

        MapDraftRequest request = new()
        {
            Factions = new List<FactionName>(),
            MapTemplate = _mapGeneratorSettingsService.MapTemplate,
            PlacementStyle = _mapGeneratorSettingsService.PlacementStyle,
            SystemWeight = _mapGeneratorSettingsService.SystemWeight,
        };

        var response = await _http.PostAsJsonAsync(uri, request);

        return await response.Content.ReadFromJsonAsync<MapDraftResult>() ?? new MapDraftResult();
    }

    public async Task InitializeSystemTilesAsync()
    {
        Uri uri = new(Paths.ApiPath_SystemTiles);

        _systemTiles = await _http.GetFromJsonAsync<IReadOnlyCollection<SystemTile>>(uri) ?? new List<SystemTile>();
    }

    public IReadOnlyCollection<SystemTile> GetSystemTilesToShow(SystemTileTypeFilter systemTileType)
    {
        return systemTileType switch
        {
            SystemTileTypeFilter.Unused => _systemTiles.ToList(),
            SystemTileTypeFilter.All => _systemTiles.ToList(),
            SystemTileTypeFilter.MecatolRex => _systemTiles.Where(x => x.Name == SystemTileName.Tile18).ToList(),
            SystemTileTypeFilter.HomeSystems => _systemTiles.Where(x => x.TileCategory == SystemTileCategory.Green).ToList(),
            SystemTileTypeFilter.BlueTiles => _systemTiles.Where(x => x.TileCategory == SystemTileCategory.Blue).ToList(),
            SystemTileTypeFilter.RedTiles => _systemTiles.Where(x => x.TileCategory == SystemTileCategory.Red).ToList(),
            SystemTileTypeFilter.Hyperlanes => _systemTiles.Where(x => x.TileCategory == SystemTileCategory.Hyperlance).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(systemTileType), systemTileType, null),
        };
    }
}
