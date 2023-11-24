using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class SystemTiles
{
    private List<SystemTile> _systemTiles = new();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        _systemTiles = await HttpClient.GetAsync<List<SystemTile>>(Paths.ApiPath_SystemTiles);
    }

    private string GetSystemTileImagePath(SystemTile systemTile)
    {
        return PathProvider.GetLargeTileImagePath(systemTile.Name.ToString());
    }
}
