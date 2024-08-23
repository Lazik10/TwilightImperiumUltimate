using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceHexTileMenu
{
    private bool _showTileMenu;
    private SystemTileTypeFilter _selectedSystemTileType = SystemTileTypeFilter.Unused;
    private IReadOnlyCollection<KeyValuePair<SystemTileTypeFilter, string>> _systemTileTypes = default!;

    [Parameter]
    public IEnumerable<SystemTileModel> SystemTiles { get; set; } = new List<SystemTileModel>();

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    public void RefreshSystemTilesMenu()
    {
        GetSystemTilesToShow();
        SwappedSystemTileFromMenu.InvokeAsync();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        _systemTileTypes = EnumExtensions.GetEnumValuesWithDisplayNames<SystemTileTypeFilter>();
        await SliceGeneratorService.InitializeAllSystemTilesForSliceGenerator();
        GetSystemTilesToShow();
    }

    private void GetSystemTilesToShow()
    {
        SystemTiles = SliceGeneratorService.GetSystemTilesToShow(_selectedSystemTileType);
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