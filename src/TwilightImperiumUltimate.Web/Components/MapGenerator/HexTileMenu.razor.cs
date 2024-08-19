using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class HexTileMenu
{
    private bool _showTileMenu;
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

    private void ToggleTileMenu()
    {
        GetSystemTilesToShow();
        _showTileMenu = !_showTileMenu;
    }

    private string GetButtonText()
    {
        return _showTileMenu ? Strings.MapGenerator_ToggleTileMenuOff : Strings.MapGenerator_ToggleTileMenuOn;
    }
}
