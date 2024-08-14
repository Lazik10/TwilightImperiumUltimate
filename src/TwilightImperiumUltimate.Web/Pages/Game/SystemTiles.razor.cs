namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class SystemTiles
{
    private List<SystemTileModel> _systemTiles = new();

    private GameVersion? _currentGameVersion;

    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        await InitializeSystemTiles();
    }

    private string GetSystemTileImagePath(SystemTileModel systemTile)
    {
        return PathProvider.GetLargeTileImagePath(systemTile.SystemTileName);
    }

    private async Task InitializeSystemTiles()
    {
        var (response, statusCode) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
        {
            var systemTiles = Mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
            _systemTiles = systemTiles
                .OrderBy(x => x.GameVersion)
                .ThenBy(x => x.SystemTileName)
                .ToList();
        }
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
