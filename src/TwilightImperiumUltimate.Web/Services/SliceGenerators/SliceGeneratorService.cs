using TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public class SliceGeneratorService(
    ITwilightImperiumApiHttpClient httpClient,
    ISliceGeneratorSettingsService sliceGeneratorSettingsService,
    IMapper mapper)
    : ISliceGeneratorService
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;
    private readonly ISliceGeneratorSettingsService _sliceGeneratorSettingsService = sliceGeneratorSettingsService;
    private readonly IMapper _mapper = mapper;

    private List<SystemTileModel> _allSystemTiles = new List<SystemTileModel>();

    private List<SliceModel> _slices = new List<SliceModel>();

    public IReadOnlyList<SystemTileModel> AllSystemTiles => _allSystemTiles;

    public IReadOnlyList<SliceModel> Slices => GetOrderedSlices();

    public async Task InitializeAllSystemTilesForSliceGenerator()
    {
        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
        {
            _allSystemTiles = _mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
        }
    }

    public async Task GeneratePreviewSlices()
    {
        await GenerateSlices(true);
    }

    public async Task GenerateSlices(bool previewSlices)
    {
        var request = new SliceDraftRequest(
            previewSlices,
            _sliceGeneratorSettingsService.NumberOfSlices,
            _sliceGeneratorSettingsService.GameVersions,
            _sliceGeneratorSettingsService.WormholeDensity,
            _sliceGeneratorSettingsService.NumberOfLegendaries);

        var (response, statusCode) = await _httpClient.PostAsync<SliceDraftRequest, ApiResponse<ItemListDto<SliceDto>>>(Paths.ApiPath_GenerateSlices, request);

        if (statusCode == HttpStatusCode.OK)
        {
            _slices = _mapper.Map<List<SliceModel>>(response!.Data!.Items);
        }
    }

    public Task AddSlice()
    {
        if (_slices.Count < 9)
        {
            _slices.Add(new SliceModel()
            {
                Id = _sliceGeneratorSettingsService.NumberOfSlices - 1,
                SystemTiles = new List<SystemTileModel>
                {
                    new SystemTileModel() { SystemTileName = SystemTileName.TileHome },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                },
            });
        }

        return Task.CompletedTask;
    }

    public Task RemoveSlice()
    {
        if (_slices.Count > 0)
        {
            var slice = _slices.OrderBy(x => x.Id).Last();
            _slices.Remove(slice);
        }

        return Task.CompletedTask;
    }

    public IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType)
    {
        return systemTileType switch
        {
            SystemTileTypeFilter.Unused => AllSystemTiles.Where(x => !UsedSystemTileCodes().Contains(x.SystemTileCode)),
            SystemTileTypeFilter.All => AllSystemTiles.OrderBy(x => x.GameVersion),
            SystemTileTypeFilter.MecatolRex => AllSystemTiles.Where(x => x.SystemTileName == SystemTileName.Tile18),
            SystemTileTypeFilter.HomeSystems => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Green),
            SystemTileTypeFilter.BlueTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Blue),
            SystemTileTypeFilter.RedTiles => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Red),
            SystemTileTypeFilter.Hyperlanes => AllSystemTiles.Where(x => x.SystemTileCategory == SystemTileCategory.Hyperlane),
            SystemTileTypeFilter.Anomalies => AllSystemTiles.Where(x => x.AnomalyName != AnomalyName.None),
            SystemTileTypeFilter.Empty => AllSystemTiles.Where(x => !x.HasPlanets
                && x.GameVersion != GameVersion.Custom
                && x.SystemTileCategory != SystemTileCategory.Hyperlane
                && x.FactionName == FactionName.None),
            SystemTileTypeFilter.BaseGame => AllSystemTiles.Where(x => x.GameVersion == GameVersion.BaseGame),
            SystemTileTypeFilter.ProphecyOfKings => AllSystemTiles.Where(x => x.GameVersion == GameVersion.ProphecyOfKings),
            SystemTileTypeFilter.DiscordantStars => AllSystemTiles.Where(x => x.GameVersion == GameVersion.DiscordantStars),
            SystemTileTypeFilter.UnchartedSpace => AllSystemTiles.Where(x => x.GameVersion == GameVersion.UnchartedSpace),
            SystemTileTypeFilter.Custom => AllSystemTiles.Where(x => x.GameVersion == GameVersion.Custom),
            _ => throw new ArgumentOutOfRangeException(nameof(systemTileType), systemTileType, null),
        };
    }

    private List<SliceModel> GetOrderedSlices()
    {
        return _slices.OrderBy(x => x.Id).ToList();
    }

    private List<string> UsedSystemTileCodes()
    {
        return _slices.SelectMany(x => x.SystemTiles).Select(x => x.SystemTileCode).ToList();
    }
}
