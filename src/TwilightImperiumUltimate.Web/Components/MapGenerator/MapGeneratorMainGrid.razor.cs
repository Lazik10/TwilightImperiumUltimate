using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGeneratorMainGrid
{
    private bool _showSettings;
    private bool _showTilesMenu;
    private bool _lastShowTilesMenuState;

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = default!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = default!;

    private void ToggleSettings()
    {
        _showSettings = !_showSettings;
        _lastShowTilesMenuState = _showTilesMenu;
        _showTilesMenu = false;
    }

    private void ToggleTilesMenu()
    {
        _showTilesMenu = !_showTilesMenu;
    }

    private void UpdateMapTemplate(MapTemplate mapTemplate)
    {
        StateHasChanged();
    }

    private void HideSettings()
    {
        _showSettings = false;
        RestoreMapTilesState();
    }

    private void RestoreMapTilesState()
    {
        _showTilesMenu = _lastShowTilesMenuState;
    }

    private async Task GenerateMap()
    {
        await MapGeneratorService.GenerateMapAsync();
        StateHasChanged();
    }
}
