using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class HexTileMenu
{
    private SystemTileTypeFilter _selectedSystemTileType = SystemTileTypeFilter.Unused;

    private IReadOnlyCollection<KeyValuePair<SystemTileTypeFilter, string>> _systemTileTypes = default!;

    public IEnumerable<SystemTileModel> SystemTiles { get; set; } = new List<SystemTileModel>();

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    public void RefreshSystemTilesMenu()
    {
        GetSystemTilesToShow();
        SwappedSystemTileFromMenu.InvokeAsync();
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        _systemTileTypes = EnumExtensions.GetEnumValuesWithDisplayNames<SystemTileTypeFilter>();
        GetSystemTilesToShow();
    }

    private void GetSystemTilesToShow()
    {
        SystemTiles = MapGeneratorService.GetSystemTilesToShow(_selectedSystemTileType);
    }
}
