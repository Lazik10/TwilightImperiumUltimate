using TwilightImperiumUltimate.Web.Services.Cache;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class SystemTiles
{
    private const int SystemTilesPageSize = 20;

    private static readonly SingleLoadCache<SystemTileModel> Cache = new();

    private List<SystemTileModel> _systemTiles = new();

    private GameVersion? _selectedGameVersion;

    private int _visibleSystemTilesCount = SystemTilesPageSize;

    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    private bool HasMoreSystemTilesToLoad => GetFilteredSystemTilesSorted().Count() > _visibleSystemTilesCount;

    protected override async Task OnInitializedAsync()
    {
        await InitializeSystemTiles();
    }

    private string GetSystemTileImagePath(SystemTileModel systemTile)
    {
        return PathProvider.GetLargeTileImagePath(systemTile.SystemTileName);
    }

    private async Task InitializeSystemTiles()
    {
        _systemTiles = (await Cache.GetOrLoadAsync(async () =>
        {
            var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
            if (statusCode != HttpStatusCode.OK)
                return [];

            var systemTiles = Mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
            return systemTiles
                .OrderBy(x => x.GameVersion)
                .ThenBy(x => x.SystemTileName)
                .ToList();
        })).ToList();
    }

    private IEnumerable<SystemTileModel> GetFilteredSystemTilesSorted()
    {
        var filteredTiles = _selectedGameVersion.HasValue
            ? _systemTiles.Where(x => x.GameVersion == _selectedGameVersion.Value)
            : _systemTiles;

        return filteredTiles
            .OrderBy(x => x.GameVersion)
            .ThenBy(x => x.SystemTileName);
    }

    private IEnumerable<IGrouping<GameVersion, SystemTileModel>> GetFilteredSystemTiles()
    {
        return GetFilteredSystemTilesSorted()
            .Take(_visibleSystemTilesCount)
            .GroupBy(x => x.GameVersion);
    }

    private Task LoadMoreSystemTiles()
    {
        _visibleSystemTilesCount += SystemTilesPageSize;
        return Task.CompletedTask;
    }

    private IEnumerable<GameVersion> GetAvailableGameVersions()
    {
        return _systemTiles
            .Select(x => x.GameVersion)
            .Distinct()
            .OrderBy(x => x);
    }

    private Task OnGameVersionFilterChanged(GameVersion? gameVersion)
    {
        _selectedGameVersion = gameVersion;
        _visibleSystemTilesCount = SystemTilesPageSize;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void ShowBigImage(SystemTileModel systemTile)
    {
        _currentBigImageSrc = GetSystemTileImagePath(systemTile);
        _showBigImage = true;
    }

    private void HideBigImage()
    {
        _showBigImage = false;
    }
}
